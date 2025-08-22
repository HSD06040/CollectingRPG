using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleAdMob : MonoBehaviour
{
    public string AdId = "ca-app-pub-3940256099942544/1033173712";
    
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

            // 광고 로드해서 메모리를 차지하고 있는 상황이니 정리 필요 => Destroy

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
        if(IsReady)
        {
            LoadedAd.Show();
        }
        else
        {
            Debug.Log("광고 로드 시작");
            Preload();
        }
    }
}
