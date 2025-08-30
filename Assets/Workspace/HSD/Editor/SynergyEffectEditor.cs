#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SynergyEffect))]
public class SynergyEffectEditor : Editor
{
    private SerializedProperty keyProp;
    private SerializedProperty descriptionProp;
    private SerializedProperty isActivationsClearProp;
    private SerializedProperty isAttackProp;
    private SerializedProperty isBuffProp;

    private SerializedProperty effectAttackTypeProp;
    private SerializedProperty powerProp;
    private SerializedProperty prefabProp;
    private SerializedProperty objRefProp;
    private SerializedProperty addressProp;

    private SerializedProperty effectTypeProp;
    private SerializedProperty synergyBuffDatasProp;
    private SerializedProperty statModifiersProp;

    private SerializedProperty triggerTypeProp;
    private SerializedProperty intervalProp;
    private SerializedProperty maxActivationsProp;

    private SerializedProperty targetTypeProp;

    private SerializedProperty nextEffectProp;

    private void OnEnable()
    {
        keyProp = serializedObject.FindProperty("Key");
        descriptionProp = serializedObject.FindProperty("Description");
        isActivationsClearProp = serializedObject.FindProperty("IsActivationsClear");
        isAttackProp = serializedObject.FindProperty("IsAttack");
        isBuffProp = serializedObject.FindProperty("IsBuff");

        effectAttackTypeProp = serializedObject.FindProperty("EffectAttackType");
        powerProp = serializedObject.FindProperty("Power");
        prefabProp = serializedObject.FindProperty("Prefab");
        objRefProp = serializedObject.FindProperty("ObjRef");
        addressProp = serializedObject.FindProperty("Address");

        effectTypeProp = serializedObject.FindProperty("EffectType");
        synergyBuffDatasProp = serializedObject.FindProperty("SynergyBuffDatas");
        statModifiersProp = serializedObject.FindProperty("StatModifiers");

        triggerTypeProp = serializedObject.FindProperty("TriggerType");
        intervalProp = serializedObject.FindProperty("Interval");
        maxActivationsProp = serializedObject.FindProperty("MaxActivations");

        targetTypeProp = serializedObject.FindProperty("TargetType");

        nextEffectProp = serializedObject.FindProperty("NextEffect");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawHeader("MetaData");
        EditorGUILayout.PropertyField(keyProp);
        EditorGUILayout.PropertyField(descriptionProp);

        EditorGUILayout.Space(5);

        EditorGUILayout.PropertyField(isActivationsClearProp);
        EditorGUILayout.PropertyField(isAttackProp);
        EditorGUILayout.PropertyField(isBuffProp);

        EditorGUILayout.Space(10);

        if (isAttackProp.boolValue)
        {
            DrawHeader("Attack Settings");

            DrawColoredSection(() => {
                EditorGUILayout.PropertyField(effectAttackTypeProp);
                DrawColoredField("Power", powerProp, Color.red);
                EditorGUILayout.PropertyField(prefabProp);
                EditorGUILayout.PropertyField(objRefProp);
                EditorGUILayout.PropertyField(addressProp);
            }, new Color(1f, 0.9f, 0.9f, 0.3f));

            EditorGUILayout.Space(10);
        }

        if (isBuffProp.boolValue)
        {
            DrawHeader("Effect Settings");

            DrawColoredSection(() => {
                EditorGUILayout.PropertyField(effectTypeProp);

                EffectType currentEffectType = (EffectType)effectTypeProp.enumValueIndex;

                EditorGUILayout.Space(5);

                if (currentEffectType == EffectType.Buff_Debuff)
                {
                    DrawSubHeader("Buff/Debuff Data:", Color.green);
                    EditorGUILayout.PropertyField(synergyBuffDatasProp, true);
                }
                else if (currentEffectType == EffectType.Increase)
                {
                    DrawSubHeader("Stat Modifiers:", Color.blue);
                    EditorGUILayout.PropertyField(statModifiersProp, true);
                }
            }, new Color(0.9f, 1f, 0.9f, 0.3f));

            EditorGUILayout.Space(10);
        }

        DrawHeader("Trigger Settings");
        EditorGUILayout.PropertyField(triggerTypeProp);

        TriggerType currentTriggerType = (TriggerType)triggerTypeProp.enumValueIndex;
        if (currentTriggerType == TriggerType.OnInterval)
        {
            EditorGUILayout.Space(5);
            EditorGUILayout.PropertyField(intervalProp, new GUIContent("Interval (seconds)"));
        }

        if (!isActivationsClearProp.boolValue)
        {
            EditorGUILayout.Space(5);
            EditorGUILayout.PropertyField(maxActivationsProp, new GUIContent("Max Activations"));
        }

        EditorGUILayout.Space(10);

        DrawHeader("Target Settings");
        EditorGUILayout.PropertyField(targetTypeProp);

        DrawTargetTypeInfo((EffectTargetType)targetTypeProp.enumValueIndex);

        EditorGUILayout.Space(10);

        DrawHeader("Chain Effect");
        EditorGUILayout.PropertyField(nextEffectProp, new GUIContent("Next Effect (Optional)"));

        if (nextEffectProp.objectReferenceValue == target)
        {
            DrawWarningBox("Warning: Circular reference detected! This effect points to itself.");
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawTargetTypeInfo(EffectTargetType targetType)
    {
        var (info, color) = targetType switch
        {
            EffectTargetType.Enemy => ("Targets enemy units", new Color(1f, 0.9f, 0.9f, 1f)),
            EffectTargetType.Ally => ("Targets all allied units", new Color(0.9f, 1f, 0.9f, 1f)),
            EffectTargetType.SameSynergy => ("Targets units with same synergy", new Color(0.9f, 0.9f, 1f, 1f)),
            EffectTargetType.Column => ("Targets entire columns where synergy units exist", new Color(1f, 1f, 0.9f, 1f)),
            EffectTargetType.Row => ("Targets entire rows where synergy units exist", new Color(1f, 0.9f, 1f, 1f)),
            EffectTargetType.ColumnAndRow => ("Targets both columns AND rows where synergy units exist", new Color(0.9f, 1f, 1f, 1f)),
            EffectTargetType.Cross => ("Targets adjacent units (cross pattern) around synergy units", new Color(1f, 1f, 1f, 1f)),
            _ => ("", Color.white)
        };

        if (!string.IsNullOrEmpty(info))
        {
            DrawInfoBox(info, color);
        }
    }

    private void DrawHeader(string title, Color? backgroundColor = null)
    {
        EditorGUILayout.Space(5);
        var rect = EditorGUILayout.GetControlRect(false, 25);

        // 기본 색상 또는 지정된 색상 사용
        Color bgColor = backgroundColor ?? GetHeaderColor(title);
        EditorGUI.DrawRect(rect, bgColor);

        // 테두리 그리기
        EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width, 1), Color.black);
        EditorGUI.DrawRect(new Rect(rect.x, rect.y + rect.height - 1, rect.width, 1), Color.black);

        var labelRect = new Rect(rect.x + 10, rect.y + 2, rect.width - 20, rect.height - 4);

        // 텍스트 스타일 설정
        var headerStyle = new GUIStyle(EditorStyles.boldLabel);
        headerStyle.normal.textColor = Color.white;
        headerStyle.fontSize = 12;

        EditorGUI.LabelField(labelRect, title, headerStyle);
    }

    private Color GetHeaderColor(string title)
    {
        return title switch
        {
            "MetaData" => new Color(0.2f, 0.4f, 0.8f, 0.8f),        // 파란색
            "Attack Settings" => new Color(0.8f, 0.2f, 0.2f, 0.8f), // 빨간색
            "Effect Settings" => new Color(0.2f, 0.8f, 0.2f, 0.8f),  // 초록색
            "Trigger Settings" => new Color(0.8f, 0.6f, 0.2f, 0.8f), // 주황색
            "Target Settings" => new Color(0.6f, 0.2f, 0.8f, 0.8f),  // 보라색
            "Chain Effect" => new Color(0.4f, 0.7f, 0.7f, 0.8f),     // 청록색
            _ => new Color(0.3f, 0.3f, 0.3f, 0.8f)                   // 기본 회색
        };
    }

    private void DrawColoredSection(System.Action drawContent, Color backgroundColor)
    {
        var rect = EditorGUILayout.BeginVertical();
        GUI.backgroundColor = backgroundColor;
        GUILayout.BeginVertical(EditorStyles.helpBox);
        GUI.backgroundColor = Color.white;

        drawContent?.Invoke();

        GUILayout.EndVertical();
        EditorGUILayout.EndVertical();
    }

    private void DrawColoredField(string label, SerializedProperty property, Color labelColor)
    {
        EditorGUILayout.BeginHorizontal();

        var originalColor = GUI.color;
        GUI.color = labelColor;
        GUILayout.Label(label, GUILayout.Width(EditorGUIUtility.labelWidth - 4));
        GUI.color = originalColor;

        EditorGUILayout.PropertyField(property, GUIContent.none);
        EditorGUILayout.EndHorizontal();
    }

    private void DrawSubHeader(string title, Color textColor)
    {
        var style = new GUIStyle(EditorStyles.miniBoldLabel);
        style.normal.textColor = textColor;
        EditorGUILayout.LabelField(title, style);
    }

    private void DrawWarningBox(string message)
    {
        var originalColor = GUI.backgroundColor;
        GUI.backgroundColor = new Color(1f, 0.8f, 0.8f, 1f);
        EditorGUILayout.HelpBox(message, MessageType.Warning);
        GUI.backgroundColor = originalColor;
    }

    private void DrawInfoBox(string message, Color backgroundColor)
    {
        var originalColor = GUI.backgroundColor;
        GUI.backgroundColor = backgroundColor;
        EditorGUILayout.HelpBox(message, MessageType.Info);
        GUI.backgroundColor = originalColor;
    }
}
#endif