#if UNITY_EDITOR
using TMPro;
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class AreaBackgroundLoader : MonoBehaviour
{
    public TextMeshProUGUI stageTextUI; 

    void Start()
    {
        if (stageTextUI != null)
        {
            string stageText = stageTextUI.text;
            LoadAreaByStageText(stageText);
        }
    }
    /// <summary>
    /// "STAGE : 4-15"와 같은 텍스트에서 스테이지 번호를 파싱하여 Area 씬을 로드합니다.
    /// </summary>
    public void LoadAreaByStageText(string stageText)
    {
        // 예시 입력: "STAGE : 4-15"
        var parts = stageText.Split(':');
        if (parts.Length < 2)
        {
            Debug.LogError("스테이지 텍스트 형식이 올바르지 않습니다: " + stageText);
            return;
        }

        var nums = parts[1].Trim().Split('-');
        if (nums.Length < 2)
        {
            Debug.LogError("스테이지 번호 형식이 올바르지 않습니다: " + stageText);
            return;
        }

        if (int.TryParse(nums[0], out int mainStage) && int.TryParse(nums[1], out int subStage))
        {
            LoadAreaByStage(mainStage, subStage);
        }
        else
        {
            Debug.LogError("스테이지 번호 파싱 실패: " + stageText);
        }
    }

    /// <summary>
    /// 메인 스테이지와 세부 스테이지를 받아서 Area 씬을 Additive로 로드합니다.
    /// </summary>
    public void LoadAreaByStage(int mainStage, int subStage)
    {
        // mainStage: 1~7, subStage: 1~15
        if (mainStage < 1 || mainStage > 7)
        {
            Debug.LogError("메인 스테이지는 1~7 사이여야 합니다.");
            return;
        }
        if (subStage < 1 || subStage > 15)
        {
            Debug.LogError("세부 스테이지는 1~15 사이여야 합니다.");
            return;
        }

        string sceneName = $"Area{mainStage}";
        if (!SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }

        // 필요하다면 subStage에 따라 추가 연출/데이터 처리 가능
        // 예: 스테이지 UI 갱신, 몬스터/배경 변화 등
    }

    /// <summary>
    /// Area1 씬을 Additive로 로드합니다.
    /// </summary>
    public void LoadAreaBackground()
    {
        SceneManager.LoadScene("Area1", LoadSceneMode.Additive);
    }
}