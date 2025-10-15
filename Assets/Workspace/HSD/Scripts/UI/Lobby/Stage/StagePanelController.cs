using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StagePanelController : MonoBehaviour
{
    [SerializeField] StageRewardPanel _stageRewardPanel;
    [SerializeField] GameObject _stagePanelPrefab;
    [SerializeField] int _stagePanelCount;
    [SerializeField] PanelSwiper _swiper;
    [SerializeField] RectTransform _content;
    [SerializeField] Button _closeButton;
    [SerializeField] Button _applyButton;
    [SerializeField] GameObject _partySelectPanel;
    [SerializeField] private BottomPanelController _bottomPanelCtrl;
    public static StageSelectButton StageSelectButton;

    private RectTransform _rectTransform;

    private void Awake()
    {
        Init();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    [ContextMenu("Init")]
    public void Init()
    {
        _rectTransform = (RectTransform)transform;

        for (int i = 0; i < _stagePanelCount; i++)
        {
            StagePanel _stagePanel = Instantiate(_stagePanelPrefab, _content).GetComponent<StagePanel>();
            _stagePanel.Init(Manager.Data.StageDatas.GetStage(i + 1), _stageRewardPanel);
            SetPosition((RectTransform)_stagePanel.transform, i);
        }

        _swiper.Init(_content, _stagePanelCount);

        _closeButton.onClick.AddListener(Close);
        _applyButton.onClick.AddListener(TryEnterPartySelectPanel);

        Canvas.ForceUpdateCanvases();
    }

    private void SetPosition(RectTransform rectTransform, int idx)
    {
        float width = _rectTransform.rect.width;
        rectTransform.anchoredPosition = new Vector2(width * idx, 0);
    }

    private void TryEnterPartySelectPanel()
    {
        for(int i = 0; i < Manager.Data.PresetDB.PresetData.Count; i++)
        {
            if (Manager.Data.PresetDB.PresetData[i].Statuses[0].Data != null)
            {
                _partySelectPanel.SetActive(true);
                return;
            }
        }

        if(PopupManager.Instance != null)
        {
            PopupManager.instance.ShowConfirmationPopup("편성된 프리셋이 없습니다.\n 파티 편성창으로 이동하겠습니까?", () => ArrangePreset());
        }
    }

    private void ArrangePreset()
    {
        _bottomPanelCtrl.SelectButton(3);
        _stagePanelPrefab.SetActive(false);
        gameObject.SetActive(false);
    }
}
