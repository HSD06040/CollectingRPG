#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class HierarchyColorizer
{
    private static readonly Color redColor = new Color(1f, 0.3f, 0.3f, 0.3f);
    private static readonly Color blueColor = new Color(0.3f, 0.5f, 1f, 0.3f);
    private static readonly Color greenColor = new Color(0.3f, 1f, 0.3f, 0.3f);
    private static readonly Color yellowColor = new Color(1f, 1f, 0.3f, 0.3f);
    private static readonly Color purpleColor = new Color(0.8f, 0.3f, 1f, 0.3f);
    private static readonly Color orangeColor = new Color(1f, 0.6f, 0.2f, 0.3f);

    static HierarchyColorizer()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
    }

    private static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (gameObject == null)
            return;

        HierarchyColorTag colorTag = gameObject.GetComponent<HierarchyColorTag>();
        if (colorTag != null && colorTag.colorType != HierarchyColorTag.ColorType.None)
        {
            if (!Selection.Contains(instanceID))
            {
                Color bgColor = GetColorByType(colorTag.colorType);
                EditorGUI.DrawRect(selectionRect, bgColor);

                var content = EditorGUIUtility.ObjectContent(gameObject, typeof(GameObject));
                EditorGUI.LabelField(selectionRect, content);
            }
        }
    }  

    private static Color GetColorByType(HierarchyColorTag.ColorType colorType)
    {
        switch (colorType)
        {
            case HierarchyColorTag.ColorType.Red: return redColor;
            case HierarchyColorTag.ColorType.Blue: return blueColor;
            case HierarchyColorTag.ColorType.Green: return greenColor;
            case HierarchyColorTag.ColorType.Yellow: return yellowColor;
            case HierarchyColorTag.ColorType.Purple: return purpleColor;
            case HierarchyColorTag.ColorType.Orange: return orangeColor;
            default: return Color.clear;
        }
    }
}

[CustomEditor(typeof(HierarchyColorTag))]
public class HierarchyColorTagEditor : Editor
{
    public override void OnInspectorGUI()
    {
        HierarchyColorTag colorTag = (HierarchyColorTag)target;

        EditorGUILayout.LabelField("Hierarchy Color Settings", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        colorTag.colorType = (HierarchyColorTag.ColorType)EditorGUILayout.EnumPopup("Color Type", colorTag.colorType);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(colorTag);
            EditorApplication.RepaintHierarchyWindow();
        }

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Hierarchy 창에서 오브젝트 배경색을 변경합니다.\n이름 패턴 방식을 사용 중이면 이 컴포넌트는 무시됩니다.", MessageType.Info);
    }
}
#endif