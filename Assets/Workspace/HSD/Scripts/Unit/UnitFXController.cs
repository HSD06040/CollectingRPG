using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFXController
{
    private SpriteRenderer[] _renderers;
    private Color[] _baseColors;
    private const float HIT_DURATION = .1f;
    private readonly Color hitColor = new Color(1f, 0.447f, 0.447f);

    public UnitFXController(SpriteRenderer[] renderers)
    {
        _renderers = renderers;

        _baseColors = new Color[_renderers.Length];
        for (int i = 0; i < _renderers.Length; i++)
        {
            _baseColors[i] = _renderers[i].color;
        }
    }    

    public void Flash()
    {
        FlashRoutine().Forget();
    }

    private async UniTask FlashRoutine()
    {
        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].color = hitColor;
        }

        await UniTask.WaitForSeconds(HIT_DURATION);

        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].color = _baseColors[i];
        }
    }
}
