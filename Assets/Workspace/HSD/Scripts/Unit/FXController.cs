using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXController
{
    private SpriteRenderer[] _renderers;
    private MaterialPropertyBlock _mpb;
    private const float HIT_DURATION = .1f;
    private readonly Color hitColor = new Color(1f, 0.447f, 0.447f);

    public FXController(SpriteRenderer[] renderers)
    {
        this._renderers = renderers;
        _mpb = new MaterialPropertyBlock();
    }    

    public void Flash()
    {
        FlashRoutine().Forget();
    }

    private async UniTask FlashRoutine()
    {
        foreach (var r in _renderers)
        {
            r.GetPropertyBlock(_mpb);
            _mpb.SetColor("_Color", hitColor);
            r.SetPropertyBlock(_mpb);
        }

        await UniTask.WaitForSeconds(HIT_DURATION);

        foreach (var r in _renderers)
        {
            r.GetPropertyBlock(_mpb);
            _mpb.SetColor("_Color", Color.white);
            r.SetPropertyBlock(_mpb);
        }
    }
}
