using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestItem : MonoBehaviour
{
    [Header("Point")]
    [SerializeField] private TMP_Text _pointText;

    [Header("Info")]
    [SerializeField] private TMP_Text _questDesc;
    [SerializeField] private Image _progressBar;

    [Header("Condition")]
    [SerializeField] private Button _questButton;
    [SerializeField] private Image _questBoxImage;
    [SerializeField] private GameObject _BadgeImage;
    [SerializeField] private TMP_Text _conditionText;


    public void Init(IQuestView questData)
    {
        // TODO: [CYH] 정렬

        _pointText.text = questData.RewardPoint.ToString();
        _questDesc.text = questData.QuestDesc;

        _progressBar.fillAmount = (float)questData.CurProgress / questData.MaxProgress;
        _conditionText.text = $"{questData.CurProgress} / {questData.MaxProgress}";

        // 클리어 퀘스트
        if (questData.IsReceived)
        {
            _questButton.interactable = false;
        }

        if (questData.IsComplete && !questData.IsReceived)
        {
            _questBoxImage.color = Color.red;
            _BadgeImage.SetActive(true);
        }

        _questButton.onClick.RemoveAllListeners();
        _questButton.onClick.AddListener(() =>
        {
            // TODO: [CYH] totalPoint 획득
        });
    }
}
