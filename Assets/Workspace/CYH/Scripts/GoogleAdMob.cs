using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleAdMob : MonoBehaviour
{
    public string AdId = "ca-app-pub-7963887056991391/5905095965";

    public InterstitialAd LoadedAd;
    private bool _isLoading;

    public bool IsReady => LoadedAd != null && LoadedAd.CanShowAd();


    private void Awake()
    {
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("광고 초기화 성공");
            Preload();
        });
    }

    private void Preload()
    {
        if (_isLoading || IsReady) return;
        _isLoading = true;

        AdRequest adRequest = new AdRequest();
        InterstitialAd.Load(AdId, adRequest, (ad, error) =>
        {
            _isLoading = false;

            if (ad == null || error != null)
            {
                Debug.LogError($"광고 로드 실패: {error.ToString()}");
                return;
            }

            Debug.Log("광고 로드 성공");

            LoadedAd = ad;

            LoadedAd.OnAdFullScreenContentClosed += () =>
            {
                LoadedAd.Destroy();
                LoadedAd = null;
                Preload();
            };

            LoadedAd.OnAdFullScreenContentFailed += err =>
            {
                LoadedAd.Destroy();
                LoadedAd = null;
                Preload();
            };

            Debug.Log("광고 로드 완료");
        });
    }

    public async void ShowAd()
    {
        if (IsReady)
        {
            LoadedAd.Show();
            // 광고 시청 보상: Diamond 20 증가
            await Manager.DB.AddDiamondAsync(20);
            QuestManager.Instance.OnAdWatched(QuestManager.Instance.adWatchQuest, true);
        }
        else
        {
            Debug.Log("광고 로드 시작");
            Preload();
        }
    }

    public void ShowAd(Action onAdWatched = null)
    {
        if (IsReady)
        {
            LoadedAd.OnAdFullScreenContentClosed += () =>
            {
                onAdWatched?.Invoke();
                LoadedAd.Destroy();
                LoadedAd = null;
                Preload();
            };

            LoadedAd.OnAdFullScreenContentFailed += err =>
            {
                Debug.LogError($"광고 닫힘 오류: {err}");
                LoadedAd.Destroy();
                LoadedAd = null;
                Preload();
            };

            LoadedAd.Show();
        }
        else
        {
            Debug.Log("광고 준비 안 됨 / Preload 실행");
            Preload();
        }
    }
}
