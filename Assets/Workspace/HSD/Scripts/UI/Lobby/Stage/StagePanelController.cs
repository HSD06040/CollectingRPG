using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagePanelController : MonoBehaviour
{
    [SerializeField] StagePanel[] _stagePanels;
    [SerializeField] PanelSwiper _swiper;
    private RectTransform _rectTransform;    

    public void Init()
    {
        _rectTransform = (RectTransform)transform;

        for (int i = 0; i < _stagePanels.Length; i++)
        {
            _stagePanels[i].Init(Manager.Data.StageDatas.GetStage(i));
            SetPosition((RectTransform)_stagePanels[i].transform, i);
        }

        _swiper.Init(_rectTransform, _stagePanels.Length);
    }

    private void SetPosition(RectTransform rectTransform, int idx)
    {
        float width = _rectTransform.rect.width;
        rectTransform.anchoredPosition = new Vector2(width * idx, 0);
    }
}
