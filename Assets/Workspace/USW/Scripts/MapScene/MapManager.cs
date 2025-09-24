using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

public class MapManager : MonoBehaviour
{
    [Header("Map Settings")]
    [SerializeField] private MapConfig _config;
    [SerializeField] private MapView _view;

    public Map CurrentMap { get; private set; }

    private void Start()
    {
        if (PlayerPrefs.HasKey("Map"))
        {
            string mapJson = PlayerPrefs.GetString("Map");
            Map map = JsonConvert.DeserializeObject<Map>(mapJson);
            
            // 보스에 도달했는지 확인
            if (map.path.Any(p => p.Equals(map.GetBossNode().point)))
            {
                // 보스 클리어 시 새 맵 생성
                Debug.Log("보스 클리어! 새로운 Stage Map 생성");
                GenerateNewMap();
            }
            else
            {
                // 기존 맵 로드
                CurrentMap = map;
                _view.ShowMap(map);
            }
        }
        else
        {
            GenerateNewMap();
        }
    }

    public void GenerateNewMap()
    {
        // StageMapGenerator만 사용
        Map map = StageMapGenerator.GetCustomMap(_config);
        Debug.Log("StageMapGenerator로 맵 생성 완료!");
        
        CurrentMap = map;
        Debug.Log(map.ToJson());
        _view.ShowMap(map);
    }

    public void SaveMap()
    {
        if (CurrentMap == null) return;

        string json = JsonConvert.SerializeObject(CurrentMap, Formatting.Indented,
            new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
        PlayerPrefs.SetString("Map", json);
        PlayerPrefs.Save();
        
        Debug.Log("Stage Map 저장 완료");
    }

    private void OnApplicationQuit()
    {
        SaveMap();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            SaveMap();
    }

    // 에디터 테스트 메서드
    [ContextMenu("Generate New Stage Map")]
    void TestStageMapGeneration()
    {
        GenerateNewMap();
    }
    
    [ContextMenu("Clear Saved Map")]
    void ClearSavedMap()
    {
        PlayerPrefs.DeleteKey("Map");
        Debug.Log("저장된 Stage Map 데이터 삭제 완료");
    }
}