#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

[InitializeOnLoad]
public class CompleteHierarchyOverride
{
    private static Dictionary<int, Texture2D> gradientTextures = new Dictionary<int, Texture2D>();
    private static HierarchyColorRules colorRules;

    static CompleteHierarchyOverride()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
        EditorApplication.hierarchyChanged += ClearTextureCache;
        LoadColorRules();
    }

    private static void LoadColorRules()
    {
        // ScriptableObject 찾기
        string[] guids = AssetDatabase.FindAssets("t:HierarchyColorRules");
        if (guids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            colorRules = AssetDatabase.LoadAssetAtPath<HierarchyColorRules>(path);
        }

        // 없으면 기본 생성
        if (colorRules == null)
        {
            colorRules = ScriptableObject.CreateInstance<HierarchyColorRules>();
            AssetDatabase.CreateAsset(colorRules, "Assets/HierarchyColorRules.asset");
            AssetDatabase.SaveAssets();
            Debug.Log("Created default HierarchyColorRules at Assets/HierarchyColorRules.asset");
        }
    }

    private static void ClearTextureCache()
    {
        foreach (var texture in gradientTextures.Values)
        {
            if (texture != null)
                Object.DestroyImmediate(texture);
        }
        gradientTextures.Clear();
    }

    private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
    {
        GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (gameObject == null) return;

        // 선택된 오브젝트는 Unity 기본 스타일 유지하지만 컴포넌트 아이콘은 그리기
        if (Selection.Contains(instanceID))
        {
            DrawComponentIcons(gameObject, selectionRect);
            return;
        }

        // 규칙에 따른 색상 감지
        Color detectedColor = GetColorByRules(gameObject.name);

        // 매칭되는 규칙이 있을 때만 커스텀 그리기
        if (detectedColor != Color.clear)
        {
            // 완전히 새로운 배경으로 덮어쓰기
            DrawCompleteBackground(selectionRect, detectedColor);

            // 완전히 새로운 내용으로 그리기
            DrawCompleteContent(gameObject, selectionRect, detectedColor);
        }

        // 모든 오브젝트에 컴포넌트 아이콘 그리기
        DrawComponentIcons(gameObject, selectionRect);
        // detectedColor가 Color.clear면 Unity 기본 렌더링 그대로 사용
    }

    private static Color GetColorByRules(string gameObjectName)
    {
        if (colorRules == null || colorRules.rules == null) return Color.clear;

        var matchedRules = new List<HierarchyColorRules.ColorRule>();

        foreach (var rule in colorRules.rules)
        {
            if (DoesNameMatchRule(gameObjectName, rule))
            {
                matchedRules.Add(rule);
            }
        }

        // 우선순위가 가장 높은 규칙 선택
        if (matchedRules.Count > 0)
        {
            var bestRule = matchedRules.OrderByDescending(r => r.priority).First();
            return bestRule.backgroundColor;
        }

        return Color.clear;
    }

    private static bool DoesNameMatchRule(string name, HierarchyColorRules.ColorRule rule)
    {
        if (string.IsNullOrEmpty(name)) return false;

        string nameToCheck = rule.caseSensitive ? name : name.ToUpper();

        // 첫 글자 패턴 체크
        if (!string.IsNullOrEmpty(rule.firstCharPattern))
        {
            string pattern = rule.caseSensitive ? rule.firstCharPattern : rule.firstCharPattern.ToUpper();
            if (nameToCheck.StartsWith(pattern))
                return true;
        }

        // 접두사 패턴 체크
        if (!string.IsNullOrEmpty(rule.prefixPattern))
        {
            string pattern = rule.caseSensitive ? rule.prefixPattern : rule.prefixPattern.ToUpper();
            if (nameToCheck.StartsWith(pattern))
                return true;
        }

        // 접미사 패턴 체크
        if (!string.IsNullOrEmpty(rule.suffixPattern))
        {
            string pattern = rule.caseSensitive ? rule.suffixPattern : rule.suffixPattern.ToUpper();
            if (nameToCheck.EndsWith(pattern))
                return true;
        }

        // 포함 패턴 체크
        if (!string.IsNullOrEmpty(rule.containsPattern))
        {
            string pattern = rule.caseSensitive ? rule.containsPattern : rule.containsPattern.ToUpper();
            if (nameToCheck.Contains(pattern))
                return true;
        }

        return false;
    }

    private static void DrawCompleteBackground(Rect rect, Color backgroundColor)
    {
        // 기본 배경으로 덮기
        Color defaultBg = EditorGUIUtility.isProSkin ?
            new Color(0.22f, 0.22f, 0.22f, 1f) :
            new Color(0.76f, 0.76f, 0.76f, 1f);
        EditorGUI.DrawRect(rect, defaultBg);

        // 그라데이션 배경 그리기
        DrawGradientRect(rect, backgroundColor);
    }

    private static void DrawGradientRect(Rect rect, Color baseColor)
    {
        int textureKey = baseColor.GetHashCode();

        if (!gradientTextures.ContainsKey(textureKey))
        {
            CreateGradientTexture(textureKey, baseColor);
        }

        if (gradientTextures.ContainsKey(textureKey) && gradientTextures[textureKey] != null)
        {
            // 그라데이션 텍스처가 제대로 적용되도록 GUI 상태 설정
            var oldColor = GUI.color;
            GUI.color = Color.white; // 텍스처 색상이 제대로 나오도록

            GUI.DrawTexture(rect, gradientTextures[textureKey], ScaleMode.StretchToFill, true);

            GUI.color = oldColor;
        }
        else
        {
            // 텍스처가 없으면 단색 배경으로 fallback
            EditorGUI.DrawRect(rect, baseColor);
        }
    }

    private static void CreateGradientTexture(int key, Color baseColor)
    {
        Texture2D gradientTexture = new Texture2D(256, 1);

        for (int i = 0; i < 256; i++)
        {
            float t = i / 255f;

            Color leftColor = Color.Lerp(baseColor * 0.8f, baseColor * 0.3f, 0.5f);
            Color rightColor = Color.Lerp(baseColor, Color.white, 0.1f);

            float curve = Mathf.Pow(t, 1.2f);
            Color pixelColor = Color.Lerp(leftColor, rightColor, curve);

            pixelColor.a = baseColor.a * Mathf.Lerp(1f, 0f, t);

            gradientTexture.SetPixel(i, 0, pixelColor);
        }

        gradientTexture.Apply();
        gradientTextures[key] = gradientTexture;
    }

    private static void DrawCompleteContent(GameObject gameObject, Rect rect, Color backgroundColor)
    {
        var content = EditorGUIUtility.ObjectContent(gameObject, typeof(GameObject));
        Color textColor = GetOptimalTextColor(backgroundColor);

        // 색상 코드 제거한 이름 가져오기
        string displayName = GetDisplayName(gameObject.name);

        GUIStyle customStyle = new GUIStyle(EditorStyles.label);
        customStyle.normal.textColor = textColor;
        customStyle.fontStyle = FontStyle.Bold;  // Bold로 변경
        customStyle.alignment = TextAnchor.MiddleCenter;  // 중앙정렬로 변경
        customStyle.richText = true;

        Rect contentRect = new Rect(rect.x, rect.y, rect.width - 80, rect.height);

        if (!gameObject.activeInHierarchy)
        {
            customStyle.normal.textColor = Color.Lerp(textColor, Color.gray, 0.6f);
        }

        PrefabAssetType prefabType = PrefabUtility.GetPrefabAssetType(gameObject);
        if (prefabType != PrefabAssetType.NotAPrefab)
        {
            customStyle.normal.textColor = Color.Lerp(textColor, Color.cyan, 0.3f);
        }

        // 색상 코드가 제거된 이름과 아이콘으로 새로운 GUIContent 생성
        var customContent = new GUIContent(displayName, content.image);
        EditorGUI.LabelField(contentRect, customContent, customStyle);

        if (gameObject.transform.childCount > 0)
        {
            string childInfo = $"({gameObject.transform.childCount})";
            Rect childRect = new Rect(contentRect.x + contentRect.width - 40, contentRect.y, 40, contentRect.height);

            GUIStyle childStyle = new GUIStyle(EditorStyles.miniLabel);
            childStyle.normal.textColor = Color.Lerp(textColor, Color.gray, 0.4f);
            childStyle.alignment = TextAnchor.MiddleRight;

            EditorGUI.LabelField(childRect, childInfo, childStyle);
        }
    }

    private static string GetDisplayName(string originalName)
    {
        // 색상 코드 패턴들 제거
        string[] colorCodes = { "#R", "#G", "#B", "#Y", "#O", "#P", "#C" };

        foreach (string code in colorCodes)
        {
            if (originalName.StartsWith(code))
            {
                return originalName.Substring(code.Length);
            }
        }

        return originalName;
    }

    private static Color GetOptimalTextColor(Color backgroundColor)
    {
        //float luminance = (0.299f * backgroundColor.r + 0.587f * backgroundColor.g + 0.114f * backgroundColor.b);
        //return (luminance > 0.4f) ? Color.black : Color.white;

        return Color.white;
    }

    private static void DrawComponentIcons(GameObject gameObject, Rect rect)
    {
        var components = gameObject.GetComponents<Component>()
            .Where(c => c != null && !(c is Transform))
            .ToArray();

        if (components.Length == 0) return;

        float iconSize = 16f;
        float iconSpacing = 18f;
        int maxIcons = 4;

        float totalWidth = Mathf.Min(components.Length, maxIcons) * iconSpacing;
        float startX = rect.x + rect.width - totalWidth - 5;

        for (int i = 0; i < Mathf.Min(components.Length, maxIcons); i++)
        {
            var component = components[i];
            if (component == null) continue;

            Rect iconRect = new Rect(
                startX + (i * iconSpacing),
                rect.y + (rect.height - iconSize) * 0.5f,
                iconSize,
                iconSize
            );

            var content = EditorGUIUtility.ObjectContent(component, component.GetType());

            Color iconBgColor = new Color(0f, 0f, 0f, 0.2f);
            EditorGUI.DrawRect(new Rect(iconRect.x - 1, iconRect.y - 1, iconRect.width + 2, iconRect.height + 2), iconBgColor);

            if (GUI.Button(iconRect, content.image, GUIStyle.none))
            {
                Selection.activeGameObject = gameObject;
                EditorGUIUtility.PingObject(component);
                EditorApplication.ExecuteMenuItem("Window/General/Inspector");
            }

            if (iconRect.Contains(Event.current.mousePosition))
            {
                EditorGUI.DrawRect(iconRect, new Color(1f, 1f, 1f, 0.1f));
                GUI.tooltip = component.GetType().Name;
            }
        }

        if (components.Length > maxIcons)
        {
            Rect moreRect = new Rect(startX + (maxIcons * iconSpacing), rect.y + 2, 12, rect.height - 4);
            GUIStyle moreStyle = new GUIStyle(EditorStyles.miniLabel);
            moreStyle.alignment = TextAnchor.MiddleCenter;
            moreStyle.normal.textColor = Color.gray;
            EditorGUI.LabelField(moreRect, "+", moreStyle);
        }
    }
}
#endif