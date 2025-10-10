using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class MessagePopup : MonoBehaviour
{
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] TMP_Text _messageText;
    [SerializeField] float _fadeDuration = 1f;
    [SerializeField] float _duration = 2;
    private CancellationTokenSource _cts;

    private void Awake()
    {
        Init();
    }

    public void Show(string message)
    {        
        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        _messageText.text = message;
        Init();

        ShowAsync(_cts.Token).Forget();
    }

    private void Init()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
    }

    private async UniTask ShowAsync(CancellationToken token)
    {
        try
        {
            await _canvasGroup.FadeIn(_fadeDuration).AttachExternalCancellation(token);

            await UniTask.WaitForSeconds(_duration, true, cancellationToken: token);

            await _canvasGroup.FadeOut(_fadeDuration).AttachExternalCancellation(token);
        }
        catch (OperationCanceledException)
        {
            DOTween.Kill(_canvasGroup, true);
        }
    }
}
