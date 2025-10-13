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
    public static StageSelectButton StageSelectButton;

    private RectTransform _rectTransform;

    private void Awake()
    {
        Manager.Data.InitAsync().Forget();
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
        //_applyButton.onClick.AddListener(() => Debug.Log("Apply"));

        Canvas.ForceUpdateCanvases();
    }

    private void SetPosition(RectTransform rectTransform, int idx)
    {
        float width = _rectTransform.rect.width;
        rectTransform.anchoredPosition = new Vector2(width * idx, 0);
    }
}
