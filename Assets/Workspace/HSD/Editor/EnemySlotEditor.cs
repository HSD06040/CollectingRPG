using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class EnemySlotEditor : EditorWindow
{
    private const int GRID_WIDTH = 5;
    private const int GRID_HEIGHT = 4;
    private const int SLOT_SIZE = 60;
    private const string SAVE_PATH = "Assets/Workspace/HSD/Data/EnemySlot";
    private const string UNIT_DATA_SEARCH_PATH = "Assets/";

    private UnitData selectedUnitData;
    private UnitGridDataSO currentGridData;
    private UnitGridDataSO[] availableGridDatas;
    private UnitData[] availableUnitDatas;
    private Vector2 scrollPosition;
    private Vector2 gridListScrollPosition;
    private Vector2 unitDataListScrollPosition;
    private string newGridName = "새그리드구성";
    private bool showUnitDataList = true;

    [MenuItem("Collecting_RPG/EnemyDataGrid_Editor")]
    public static void ShowWindow()
    {
        GetWindow<EnemySlotEditor>("유닛 데이터 그리드 편집기");
    }

    private void OnEnable()
    {
        CreateNewGridData();
        RefreshAvailableGridDatas();
        RefreshAvailableUnitDatas();
    }

    private void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        DrawUnitDataSelector();
        EditorGUILayout.Space(10);

        DrawGrid();
        EditorGUILayout.Space(10);

        DrawCreateSection();
        EditorGUILayout.Space(10);

        DrawGridDataSelector();

        EditorGUILayout.EndScrollView();
    }

    private void DrawUnitDataSelector()
    {
        EditorGUILayout.LabelField("유닛 데이터 설정", EditorStyles.boldLabel);

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
        UnitData newUnitData = (UnitData)EditorGUILayout.ObjectField(selectedUnitData, typeof(UnitData), false);
        if (newUnitData != selectedUnitData)
        {
            selectedUnitData = newUnitData;
        }
        EditorGUILayout.EndHorizontal();

        // UnitData 목록
        if (showUnitDataList)
        {
            DrawUnitDataList();
        }

        // 선택된 UnitData 미리보기
        if (selectedUnitData != null)
        {
            DrawSelectedUnitDataPreview();
        }
    }

    private void DrawGrid()
    {
        EditorGUILayout.LabelField("그리드 크기 (5x4)", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("좌클릭: UnitData 배치 | 우클릭: UnitData 삭제");

        EditorGUILayout.BeginVertical(GUI.skin.box);

        for (int y = 0; y < GRID_HEIGHT; y++)
        {
            EditorGUILayout.BeginHorizontal();

            for (int x = 0; x < GRID_WIDTH; x++)
            {
                Vector2Int position = new Vector2Int(x, y);
                UnitData unitData = currentGridData.GetUnitData(position);

                Rect slotRect = GUILayoutUtility.GetRect(SLOT_SIZE, SLOT_SIZE);

                GUI.Box(slotRect, "", GUI.skin.button);

                if (unitData != null && unitData.Icon != null)
                {
                    Rect iconRect = new Rect(slotRect.x + 2, slotRect.y + 2, slotRect.width - 4, slotRect.height - 4);
                    GUI.DrawTexture(iconRect, unitData.Icon.texture, ScaleMode.ScaleToFit);

                    // 유닛 이름과 레벨 표시 (작은 텍스트)
                    Rect textRect = new Rect(slotRect.x, slotRect.y + slotRect.height - 15, slotRect.width, 15);
                    GUI.Label(textRect, $"{unitData.Name} Lv.{unitData.Level}", EditorStyles.miniLabel);
                }

                Event currentEvent = Event.current;
                if (slotRect.Contains(currentEvent.mousePosition))
                {
                    if (currentEvent.type == EventType.MouseDown)
                    {
                        if (currentEvent.button == 0) // 좌클릭
                        {
                            if (selectedUnitData != null)
                            {
                                currentGridData.SetUnitData(position, selectedUnitData);
                                EditorUtility.SetDirty(currentGridData);
                                Repaint();
                            }
                        }
                        else if (currentEvent.button == 1) // 우클릭
                        {
                            currentGridData.RemoveUnitData(position);
                            EditorUtility.SetDirty(currentGridData);
                            Repaint();
                        }
                        currentEvent.Use();
                    }
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
    }

    private void DrawCreateSection()
    {
        EditorGUILayout.LabelField("저장하기", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("이름:", GUILayout.Width(50));
        newGridName = EditorGUILayout.TextField(newGridName);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("생성하기", GUILayout.Height(30)))
        {
            CreateGridDataAsset();
        }

        EditorGUILayout.Space(5);

        if (GUILayout.Button("그리드 전체 삭제", GUILayout.Height(25)))
        {
            if (EditorUtility.DisplayDialog("그리드 전체 삭제", "정말로 모든 그리드를 삭제하시겠습니까?", "예", "아니오"))
            {
                currentGridData.ClearAllUnitDatas();
                EditorUtility.SetDirty(currentGridData);
                Repaint();
            }
        }
    }

    private void DrawGridDataSelector()
    {
        EditorGUILayout.LabelField("불러오기", EditorStyles.boldLabel);

        if (GUILayout.Button("새로고침"))
        {
            RefreshAvailableGridDatas();
        }

        if (availableGridDatas != null && availableGridDatas.Length > 0)
        {
            gridListScrollPosition = EditorGUILayout.BeginScrollView(gridListScrollPosition, GUILayout.Height(150));

            foreach (UnitGridDataSO gridData in availableGridDatas)
            {
                if (gridData != null)
                {
                    EditorGUILayout.BeginHorizontal(GUI.skin.box);

                    if (GUILayout.Button(gridData.gridName, EditorStyles.label))
                    {
                        LoadGridData(gridData);
                    }

                    EditorGUILayout.LabelField($"유닛: {gridData.unitDatas.Count}", GUILayout.Width(80));

                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndScrollView();
        }
        else
        {
            EditorGUILayout.HelpBox("그리드 구성이 없습니다. 먼저 하나를 만들어주세요!", MessageType.Info);
        }
    }

    private void CreateNewGridData()
    {
        currentGridData = CreateInstance<UnitGridDataSO>();
        currentGridData.gridName = "임시 구성";
    }

    private void CreateGridDataAsset()
    {
        if (string.IsNullOrEmpty(newGridName))
        {
            EditorUtility.DisplayDialog("오류", "유효한 이름을 입력해주세요!", "확인");
            return;
        }

        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }

        string fileName = $"{newGridName}.asset";
        string fullPath = Path.Combine(SAVE_PATH, fileName);

        if (File.Exists(fullPath))
        {
            if (!EditorUtility.DisplayDialog("파일 존재", $"파일 {fileName}이(가) 이미 존재합니다. 덮어쓰시겠습니까?", "예", "아니오"))
            {
                return;
            }
        }

        UnitGridDataSO newGridData = CreateInstance<UnitGridDataSO>();
        newGridData.gridName = newGridName;
        newGridData.unitDatas.AddRange(currentGridData.unitDatas);

        AssetDatabase.CreateAsset(newGridData, fullPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("성공", $"그리드 구성이 {fileName}으로 저장되었습니다!", "확인");

        RefreshAvailableGridDatas();
    }

    private void RefreshAvailableGridDatas()
    {
        Debug.Log($"SAVE_PATH: {SAVE_PATH}");
        Debug.Log($"Directory exists: {Directory.Exists(SAVE_PATH)}");

        if (!Directory.Exists(SAVE_PATH))
        {
            availableGridDatas = new UnitGridDataSO[0];
            return;
        }

        string[] guids = AssetDatabase.FindAssets("t:UnitGridDataSO", new[] { SAVE_PATH });
        Debug.Log($"Found {guids.Length} UnitGridDataSO files");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Debug.Log($"Found asset at path: {path}");
        }

        availableGridDatas = new UnitGridDataSO[guids.Length];

        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            availableGridDatas[i] = AssetDatabase.LoadAssetAtPath<UnitGridDataSO>(path);

            if (availableGridDatas[i] == null)
            {
                Debug.LogError($"Failed to load UnitGridDataSO at path: {path}");
            }
            else
            {
                Debug.Log($"Successfully loaded: {availableGridDatas[i].gridName}");
            }
        }
    }

    private void LoadGridData(UnitGridDataSO gridData)
    {
        currentGridData.ClearAllUnitDatas();

        foreach (var unitDataInfo in gridData.unitDatas)
        {
            currentGridData.SetUnitData(unitDataInfo.position, unitDataInfo.unitData);
        }

        EditorUtility.SetDirty(currentGridData);
        Repaint();

        Debug.Log($"그리드 구성을 불러왔습니다: {gridData.gridName}");
    }

    private void RefreshAvailableUnitDatas()
    {
        string[] unitDataGuids = AssetDatabase.FindAssets("t:UnitData", new[] { UNIT_DATA_SEARCH_PATH });
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

        // 이름순으로 정렬
        System.Array.Sort(availableUnitDatas, (a, b) => a.Name.CompareTo(b.Name));
    }

    private void DrawUnitDataList()
    {
        EditorGUILayout.LabelField("UnitData 목록:", EditorStyles.miniLabel);

        if (availableUnitDatas == null || availableUnitDatas.Length == 0)
        {
            EditorGUILayout.HelpBox("UnitData ScriptableObject를 찾을 수 없습니다. UnitData SO가 프로젝트에 있는지 확인해주세요.", MessageType.Info);
            return;
        }

        unitDataListScrollPosition = EditorGUILayout.BeginScrollView(unitDataListScrollPosition, GUILayout.Height(120));

        for (int i = 0; i < availableUnitDatas.Length; i++)
        {
            if (availableUnitDatas[i] == null) continue;

            UnitData unitData = availableUnitDatas[i];

            // 선택 상태에 따른 색상 설정
            bool isSelected = selectedUnitData == unitData;
            Color originalColor = GUI.backgroundColor;
            if (isSelected)
            {
                GUI.backgroundColor = Color.cyan;
            }

            EditorGUILayout.BeginHorizontal(GUI.skin.box);

            // 아이콘 표시
            if (unitData.Icon != null)
            {
                Texture2D iconTexture = AssetPreview.GetAssetPreview(unitData.Icon);
                if (iconTexture != null)
                {
                    GUILayout.Label(iconTexture, GUILayout.Width(24), GUILayout.Height(24));
                }
                else
                {
                    GUILayout.Space(28);
                }
            }
            else
            {
                GUILayout.Space(28);
            }

            // 클릭 가능한 라벨
            if (GUILayout.Button($"{unitData.Name} (Lv.{unitData.Level})", EditorStyles.label))
            {
                selectedUnitData = unitData;
            }

            EditorGUILayout.EndHorizontal();

            // 원래 색상 복원
            GUI.backgroundColor = originalColor;
        }

        EditorGUILayout.EndScrollView();
    }

    private void DrawSelectedUnitDataPreview()
    {
        if (selectedUnitData != null)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("선택된 유닛 데이터 미리보기:", EditorStyles.miniLabel);

            EditorGUILayout.BeginHorizontal();

            // 아이콘
            if (selectedUnitData.Icon != null)
            {
                Texture2D iconTexture = AssetPreview.GetAssetPreview(selectedUnitData.Icon);
                if (iconTexture != null)
                {
                    GUILayout.Label(iconTexture, GUILayout.Width(48), GUILayout.Height(48));
                }
            }

            // 정보
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("이름:", selectedUnitData.Name);
            EditorGUILayout.LabelField("레벨:", selectedUnitData.Level.ToString());
            EditorGUILayout.LabelField("체력:", selectedUnitData.MaxHealth.ToString());
            EditorGUILayout.LabelField("물리데미지:", selectedUnitData.PhysicalDamage.ToString());
            EditorGUILayout.LabelField("마법데미지:", selectedUnitData.MagicDamage.ToString());
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
    }
}