using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StagePanelController : MonoBehaviour
{
    [SerializeField] StagePanel[] _stagePanels;
    [SerializeField] PanelSwiper _swiper;
    [SerializeField] RectTransform _content;
    [SerializeField] Button _closeButton;
    [SerializeField] Button _applyButton;

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

        for (int i = 0; i < _stagePanels.Length; i++)
        {
            _stagePanels[i].Init(Manager.Data.StageDatas.GetStage(i + 1));
            SetPosition((RectTransform)_stagePanels[i].transform, i);
        }

        _swiper.Init(_content, _stagePanels.Length);

        _closeButton.onClick.AddListener(Close);
        //_applyButton.onClick.AddListener(() => Debug.Log("Apply"));
    }

    private void SetPosition(RectTransform rectTransform, int idx)
    {
        float width = _rectTransform.rect.width;
        rectTransform.anchoredPosition = new Vector2(width * idx, 0);
    }
}
