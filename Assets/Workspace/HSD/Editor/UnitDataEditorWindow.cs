using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class UnitDataEditorWindow : EditorWindow
{
    private const string SAVE_PATH = "Assets/Workspace/HSD/Data/Unit";
    private const string UNITDATA_SEARCH_PATH = "Assets/"; // UnitData SO 검색 경로

    private UnitData currentUnitData;
    private UnitData[] availableUnitDatas;
    private Vector2 scrollPosition;
    private Vector2 unitDataListScrollPosition;
    private Vector2 mainScrollPosition;
    private string newUnitName = "새유닛";
    private bool showUnitDataList = true;

    // 폴더블 섹션들
    private bool showMetaDataSection = true;
    private bool showStatusSection = true;
    private bool showDamageSection = true;
    private bool showCritSection = true;
    private bool showDefenseSection = true;
    private bool showRangeSection = true;
    private bool showAttackDataSection = true;
    private bool showEnhancementSection = true;

    [MenuItem("Collecting_RPG/UnitData_Editor")]
    public static void ShowWindow()
    {
        GetWindow<UnitDataEditorWindow>("유닛 데이터 편집기");
    }

    private void OnEnable()
    {
        CreateNewUnitData();
        RefreshAvailableUnitDatas();
    }

    private void OnGUI()
    {
        mainScrollPosition = EditorGUILayout.BeginScrollView(mainScrollPosition);

        DrawUnitDataSelector();
        EditorGUILayout.Space(10);

        DrawUnitDataEditor();
        EditorGUILayout.Space(10);

        DrawCreateSection();
        EditorGUILayout.Space(10);

        DrawUnitDataList();

        EditorGUILayout.EndScrollView();
    }

    private void DrawUnitDataSelector()
    {
        EditorGUILayout.LabelField("유닛 데이터 선택", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("UnitData 새로고침", GUILayout.Width(150)))
        {
            RefreshAvailableUnitDatas();
        }
        showUnitDataList = EditorGUILayout.Toggle("유닛 데이터 리스트 보기", showUnitDataList);
        EditorGUILayout.EndHorizontal();

        // 수동 선택
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("수동 선택:", GUILayout.Width(100));
        UnitData newUnitData = (UnitData)EditorGUILayout.ObjectField(currentUnitData, typeof(UnitData), false);
        if (newUnitData != currentUnitData && newUnitData != null)
        {
            LoadUnitData(newUnitData);
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawUnitDataEditor()
    {
        if (currentUnitData == null) return;

        EditorGUILayout.LabelField("유닛 데이터 편집", EditorStyles.boldLabel);

        EditorGUILayout.BeginVertical(GUI.skin.box);

        // MetaData 섹션
        showMetaDataSection = EditorGUILayout.Foldout(showMetaDataSection, "MetaData", true, EditorStyles.foldoutHeader);
        if (showMetaDataSection)
        {
            EditorGUI.indentLevel++;
            currentUnitData.Level = EditorGUILayout.IntField("Level", currentUnitData.Level);
            currentUnitData.Grade = (Grade)EditorGUILayout.EnumPopup("Grade", currentUnitData.Grade);
            currentUnitData.UnitPrefab = (GameObject)EditorGUILayout.ObjectField("Unit Prefab", currentUnitData.UnitPrefab, typeof(GameObject), false);
            currentUnitData.Icon = (Sprite)EditorGUILayout.ObjectField("Icon", currentUnitData.Icon, typeof(Sprite), false);
            currentUnitData.ID = EditorGUILayout.IntField("ID", currentUnitData.ID);
            currentUnitData.Name = EditorGUILayout.TextField("Name", currentUnitData.Name);
            currentUnitData.Description = EditorGUILayout.TextArea(currentUnitData.Description, GUILayout.Height(60));
            currentUnitData.Cost = EditorGUILayout.IntField("Cost", currentUnitData.Cost);
            currentUnitData.CombatPower = EditorGUILayout.IntField("Combat Power", currentUnitData.CombatPower);
            currentUnitData.UpgradeCount = EditorGUILayout.IntField("Upgrade Count", currentUnitData.UpgradeCount);
            EditorGUI.indentLevel--;
            EditorGUILayout.Space(5);
        }

        // Status 섹션
        showStatusSection = EditorGUILayout.Foldout(showStatusSection, "Status", true, EditorStyles.foldoutHeader);
        if (showStatusSection)
        {
            EditorGUI.indentLevel++;
            currentUnitData.MaxHealth = EditorGUILayout.IntField("Max Health", currentUnitData.MaxHealth);
            currentUnitData.MaxMana = EditorGUILayout.IntField("Max Mana", currentUnitData.MaxMana);
            currentUnitData.ManaGain = EditorGUILayout.IntField("Mana Gain", currentUnitData.ManaGain);
            currentUnitData.AttackSpeed = EditorGUILayout.FloatField("Attack Speed", currentUnitData.AttackSpeed);
            currentUnitData.MoveSpeed = EditorGUILayout.FloatField("Move Speed", currentUnitData.MoveSpeed);
            EditorGUI.indentLevel--;
            EditorGUILayout.Space(5);
        }

        // Damage 섹션
        showDamageSection = EditorGUILayout.Foldout(showDamageSection, "Damage", true, EditorStyles.foldoutHeader);
        if (showDamageSection)
        {
            EditorGUI.indentLevel++;
            currentUnitData.PhysicalDamage = EditorGUILayout.IntField("Physical Damage", currentUnitData.PhysicalDamage);
            currentUnitData.MagicDamage = EditorGUILayout.IntField("Magic Damage", currentUnitData.MagicDamage);
            EditorGUI.indentLevel--;
            EditorGUILayout.Space(5);
        }

        // CritRate 섹션
        showCritSection = EditorGUILayout.Foldout(showCritSection, "Critical", true, EditorStyles.foldoutHeader);
        if (showCritSection)
        {
            EditorGUI.indentLevel++;
            currentUnitData.CritChance = EditorGUILayout.IntField("Crit Chance", currentUnitData.CritChance);
            currentUnitData.CritDamage = EditorGUILayout.IntField("Crit Damage", currentUnitData.CritDamage);
            EditorGUI.indentLevel--;
            EditorGUILayout.Space(5);
        }

        // Defense 섹션
        showDefenseSection = EditorGUILayout.Foldout(showDefenseSection, "Defense", true, EditorStyles.foldoutHeader);
        if (showDefenseSection)
        {
            EditorGUI.indentLevel++;
            currentUnitData.PhysicalDefense = EditorGUILayout.IntField("Physical Defense", currentUnitData.PhysicalDefense);
            currentUnitData.MagicDefense = EditorGUILayout.IntField("Magic Defense", currentUnitData.MagicDefense);
            EditorGUI.indentLevel--;
            EditorGUILayout.Space(5);
        }

        // Range 섹션
        showRangeSection = EditorGUILayout.Foldout(showRangeSection, "Range", true, EditorStyles.foldoutHeader);
        if (showRangeSection)
        {
            EditorGUI.indentLevel++;
            currentUnitData.AttackRange = EditorGUILayout.IntField("Attack Range", currentUnitData.AttackRange);
            currentUnitData.AttackCount = EditorGUILayout.IntField("Attack Count", currentUnitData.AttackCount);
            currentUnitData.AttackAreaType = (AttackAreaType)EditorGUILayout.EnumPopup("Attack Area Type", currentUnitData.AttackAreaType);
            EditorGUI.indentLevel--;
            EditorGUILayout.Space(5);
        }

        // Attack Data 섹션
        showAttackDataSection = EditorGUILayout.Foldout(showAttackDataSection, "Attack Data", true, EditorStyles.foldoutHeader);
        if (showAttackDataSection)
        {
            EditorGUI.indentLevel++;
            currentUnitData.Skill = (UnitSkill)EditorGUILayout.ObjectField("Skill", currentUnitData.Skill, typeof(UnitSkill), false);
            currentUnitData.AttackData = (UnitAttackData)EditorGUILayout.ObjectField("Attack Data", currentUnitData.AttackData, typeof(UnitAttackData), false);
            EditorGUI.indentLevel--;
            EditorGUILayout.Space(5);
        }

        // Enhancement 섹션
        showEnhancementSection = EditorGUILayout.Foldout(showEnhancementSection, "Player Enhancement", true, EditorStyles.foldoutHeader);
        if (showEnhancementSection)
        {
            EditorGUI.indentLevel++;
            currentUnitData.EnhancementData = (UnitEnhancementData)EditorGUILayout.ObjectField("Enhancement Data", currentUnitData.EnhancementData, typeof(UnitEnhancementData), false);
            EditorGUI.indentLevel--;
            EditorGUILayout.Space(5);
        }

        EditorGUILayout.EndVertical();

        // 변경사항이 있으면 dirty 표시
        if (GUI.changed)
        {
            EditorUtility.SetDirty(currentUnitData);
        }
    }

    private void DrawCreateSection()
    {
        EditorGUILayout.LabelField("저장하기", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("이름:", GUILayout.Width(50));
        newUnitName = EditorGUILayout.TextField(newUnitName);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("새 UnitData 생성", GUILayout.Height(30)))
        {
            CreateUnitDataAsset();
        }

        if (currentUnitData != null && GUILayout.Button("현재 데이터 저장", GUILayout.Height(30)))
        {
            SaveCurrentUnitData();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(5);

        if (GUILayout.Button("데이터 초기화", GUILayout.Height(25)))
        {
            if (EditorUtility.DisplayDialog("데이터 초기화", "정말로 현재 데이터를 초기화하시겠습니까?", "예", "아니오"))
            {
                CreateNewUnitData();
                Repaint();
            }
        }
    }

    private void DrawUnitDataList()
    {
        EditorGUILayout.LabelField("불러오기", EditorStyles.boldLabel);

        if (GUILayout.Button("새로고침"))
        {
            RefreshAvailableUnitDatas();
        }

        if (showUnitDataList)
        {
            DrawAvailableUnitDataList();
        }
    }

    private void CreateNewUnitData()
    {
        currentUnitData = CreateInstance<UnitData>();
        currentUnitData.Name = "새유닛";
        currentUnitData.Level = 1;
        currentUnitData.MaxHealth = 100;
        currentUnitData.AttackSpeed = 1.0f;
        currentUnitData.MoveSpeed = 1.0f;
    }

    private void CreateUnitDataAsset()
    {
        if (string.IsNullOrEmpty(newUnitName))
        {
            EditorUtility.DisplayDialog("오류", "유효한 이름을 입력해주세요!", "확인");
            return;
        }

        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }

        string fileName = $"{newUnitName}_1.asset";
        string fullPath = Path.Combine(SAVE_PATH, fileName);

        if (File.Exists(fullPath))
        {
            if (!EditorUtility.DisplayDialog("파일 존재", $"파일 {fileName}이(가) 이미 존재합니다. 덮어쓰시겠습니까?", "예", "아니오"))
            {
                return;
            }
        }

        UnitData newUnitData = CreateInstance<UnitData>();
        CopyUnitData(currentUnitData, newUnitData);
        newUnitData.Name = newUnitName;

        AssetDatabase.CreateAsset(newUnitData, fullPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("성공", $"유닛 데이터가 {fileName}으로 저장되었습니다!", "확인");

        RefreshAvailableUnitDatas();
        LoadUnitData(newUnitData);
    }

    private void SaveCurrentUnitData()
    {
        if (currentUnitData != null)
        {
            EditorUtility.SetDirty(currentUnitData);
            AssetDatabase.SaveAssets();
            EditorUtility.DisplayDialog("저장 완료", "현재 유닛 데이터가 저장되었습니다!", "확인");
        }
    }

    private void RefreshAvailableUnitDatas()
    {
        string[] unitDataGuids = AssetDatabase.FindAssets("t:UnitData", new[] { UNITDATA_SEARCH_PATH });
        System.Collections.Generic.List<UnitData> unitDataList = new System.Collections.Generic.List<UnitData>();

        foreach (string guid in unitDataGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            UnitData unitData = AssetDatabase.LoadAssetAtPath<UnitData>(path);

            if (unitData != null)
            {
                unitDataList.Add(unitData);
            }
        }

        availableUnitDatas = unitDataList.ToArray();
        System.Array.Sort(availableUnitDatas, (a, b) => a.Name.CompareTo(b.Name));
    }

    private void LoadUnitData(UnitData unitData)
    {
        if (unitData != null)
        {
            currentUnitData = unitData;
            Repaint();
            Debug.Log($"유닛 데이터를 불러왔습니다: {unitData.Name}");
        }
    }

    private void DrawAvailableUnitDataList()
    {
        EditorGUILayout.LabelField("UnitData 목록:", EditorStyles.miniLabel);

        if (availableUnitDatas == null || availableUnitDatas.Length == 0)
        {
            EditorGUILayout.HelpBox("UnitData ScriptableObject를 찾을 수 없습니다. UnitData SO가 프로젝트에 있는지 확인해주세요.", MessageType.Info);
            return;
        }

        unitDataListScrollPosition = EditorGUILayout.BeginScrollView(unitDataListScrollPosition, GUILayout.Height(200));

        for (int i = 0; i < availableUnitDatas.Length; i++)
        {
            if (availableUnitDatas[i] == null) continue;

            EditorGUILayout.BeginHorizontal(GUI.skin.box);

            bool isSelected = currentUnitData == availableUnitDatas[i];
            if (isSelected)
            {
                GUI.backgroundColor = Color.cyan;
            }

            UnitData unitData = availableUnitDatas[i];

            EditorGUILayout.BeginHorizontal();

            // 아이콘 표시
            if (unitData.Icon != null)
            {
                Texture2D iconTexture = AssetPreview.GetAssetPreview(unitData.Icon);
                if (iconTexture != null)
                {
                    GUILayout.Label(iconTexture, GUILayout.Width(32), GUILayout.Height(32));
                }
            }
            else
            {
                GUILayout.Space(36);
            }

            EditorGUILayout.BeginVertical();

            // 클릭 가능한 라벨
            if (GUILayout.Button($"{unitData.Name} (Lv.{unitData.Level})", EditorStyles.label))
            {
                LoadUnitData(unitData);
            }

            EditorGUILayout.LabelField($"ID: {unitData.ID} | HP: {unitData.MaxHealth} | ATK: {unitData.PhysicalDamage + unitData.MagicDamage}", EditorStyles.miniLabel);

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
    }

    private void CopyUnitData(UnitData source, UnitData target)
    {
        target.Level = source.Level;
        target.Grade = source.Grade;
        target.UnitPrefab = source.UnitPrefab;
        target.Icon = source.Icon;
        target.ID = source.ID;
        target.Name = source.Name;
        target.Description = source.Description;
        target.Cost = source.Cost;
        target.CombatPower = source.CombatPower;
        target.UpgradeCount = source.UpgradeCount;
        target.MaxHealth = source.MaxHealth;
        target.MaxMana = source.MaxMana;
        target.ManaGain = source.ManaGain;
        target.AttackSpeed = source.AttackSpeed;
        target.MoveSpeed = source.MoveSpeed;
        target.PhysicalDamage = source.PhysicalDamage;
        target.MagicDamage = source.MagicDamage;
        target.CritChance = source.CritChance;
        target.CritDamage = source.CritDamage;
        target.PhysicalDefense = source.PhysicalDefense;
        target.MagicDefense = source.MagicDefense;
        target.AttackRange = source.AttackRange;
        target.AttackCount = source.AttackCount;
        target.AttackAreaType = source.AttackAreaType;
        target.Skill = source.Skill;
        target.AttackData = source.AttackData;
        target.EnhancementData = source.EnhancementData;
    }
}