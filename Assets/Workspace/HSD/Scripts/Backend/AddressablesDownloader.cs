using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.AddressableAssets.ResourceLocators;
using System;

public class AddressablesDownloader : MonoBehaviour
{
    [Header("Download Settings")]
    public List<AssetLabelReference> _labelsToDownload;
    public bool _checkOnStart = true;

    [Header("Progress Info")]
    public long _totalFileSize;
    public long _downloadedSize;
    public float _downloadProgress;
    public bool _isDownloading;
    public bool _isChecking;

    // 다운로드가 필요할 때 호출되는 이벤트 (크기 정보 포함)
    public Action<long> OnNeedDownloading;

    private async void Start()
    {
        if (_labelsToDownload == null || _labelsToDownload.Count == 0)
        {            
            Debug.LogWarning("[어드레서블] 다운로드할 라벨이 설정되지 않았습니다. Inspector에서 라벨을 추가해주세요.");
            return;
        }

        if (_checkOnStart)
        {
            await CheckForDownloads();
        }
    }

    /// <summary>
    /// 어드레서블을 초기화하고 다운로드가 필요한지 체크만 함
    /// </summary>
    public async UniTask CheckForDownloads()
    {
        if (_isChecking || _isDownloading) return;

        _isChecking = true;

        try
        {
            await InitializeAddressables();
            await CheckCatalogUpdates();

            // 다운로드 필요한지 체크
            long downloadSize = await GetTotalDownloadSize();

            if (downloadSize > 0)
            {
                Debug.Log($"[어드레서블] 다운로드할 파일이 있습니다. 크기: {FormatBytes(downloadSize)}");
                OnNeedDownloading?.Invoke(downloadSize);
            }
            else
            {
                Debug.Log("[어드레서블] 다운로드할 파일이 없습니다. 모든 파일이 최신 상태입니다.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[어드레서블] 체크 실패: {e.Message}");
        }
        finally
        {
            _isChecking = false;
        }
    }

    /// <summary>
    /// 실제 다운로드 실행 (OnNeedDownloading 이벤트 후 사용자가 승인하면 호출)
    /// </summary>
    public async UniTask StartDownload()
    {
        if (_isDownloading || _isChecking) return;

        try
        {
            await DownloadAllLabels();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[어드레서블] 다운로드 실패: {e.Message}");
        }
    }

    private async UniTask InitializeAddressables()
    {
        var initHandle = Addressables.InitializeAsync();
        await initHandle.ToUniTask();

        if (initHandle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("[어드레서블] 초기화 완료");
        }
        else
        {
            throw new System.Exception("[어드레서블] 초기화 실패");
        }

        Addressables.Release(initHandle);
    }

    private async UniTask CheckCatalogUpdates()
    {
        var checkHandle = Addressables.CheckForCatalogUpdates(false);
        var catalogsToUpdate = await checkHandle.ToUniTask();

        if (catalogsToUpdate.Count > 0)
        {
            Debug.Log($"[어드레서블] 업데이트할 카탈로그 수: {catalogsToUpdate.Count}");

            var updateHandle = Addressables.UpdateCatalogs(catalogsToUpdate, false);
            await updateHandle.ToUniTask();

            Debug.Log("[어드레서블] 카탈로그 업데이트 완료");
            Addressables.Release(updateHandle);
        }
        else
        {
            Debug.Log("[어드레서블] 업데이트할 카탈로그 없음");
        }

        Addressables.Release(checkHandle);
    }

    private async UniTask DownloadAllLabels()
    {
        _isDownloading = true;

        try
        {
            // 모든 라벨에 대한 다운로드 사이즈 계산
            await CalculateTotalDownloadSize();

            if (_totalFileSize == 0)
            {
                Debug.Log("[어드레서블] 다운로드할 파일이 없습니다.");
                return;
            }

            Debug.Log($"[어드레서블] 다운로드 시작 - 총 크기: {FormatBytes(_totalFileSize)}");

            // 각 라벨별로 다운로드
            foreach (AssetLabelReference label in _labelsToDownload)
            {
                await DownloadLabel(label);
            }

            Debug.Log("[어드레서블] 모든 라벨 다운로드 완료");
        }
        finally
        {
            _isDownloading = false;
        }
    }

    private async UniTask CalculateTotalDownloadSize()
    {
        _totalFileSize = 0;

        foreach (AssetLabelReference label in _labelsToDownload)
        {
            var sizeHandle = Addressables.GetDownloadSizeAsync(label);
            long labelSize = await sizeHandle.ToUniTask();
            _totalFileSize += labelSize;

            if (labelSize > 0)
            {
                Debug.Log($"[어드레서블] '{label}' 라벨 다운로드 크기: {FormatBytes(labelSize)}");
            }

            Addressables.Release(sizeHandle);
        }

        Debug.Log($"[어드레서블] 전체 다운로드 크기: {FormatBytes(_totalFileSize)}");
    }

    private async UniTask DownloadLabel(AssetLabelReference label)
    {
        // 해당 라벨의 다운로드 사이즈 확인
        var sizeHandle = Addressables.GetDownloadSizeAsync(label);
        long labelSize = await sizeHandle.ToUniTask();
        Addressables.Release(sizeHandle);

        if (labelSize > 0)
        {
            Debug.Log($"[어드레서블] {label} 다운로드 시작");

            var downloadHandle = Addressables.DownloadDependenciesAsync(label, false);

            // 진행률 모니터링
            while (!downloadHandle.IsDone)
            {
                if (downloadHandle.IsValid())
                {
                    var status = downloadHandle.GetDownloadStatus();
                    _downloadProgress = downloadHandle.PercentComplete;
                    _downloadedSize = (long)(_totalFileSize * _downloadProgress);

                    Debug.Log($"[어드레서블] 다운로드 중 {label}: {_downloadProgress:P2} - {FormatBytes(_downloadedSize)}/{FormatBytes(_totalFileSize)}");
                }

                await UniTask.Yield();
            }

            if (downloadHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log($"[어드레서블] {label} 다운로드 완료");
            }
            else
            {
                Debug.LogError($"[어드레서블] {label} 다운로드 실패: {downloadHandle.OperationException}");
            }

            Addressables.Release(downloadHandle);
        }
        else
        {
            Debug.Log($"[어드레서블] {label} 라벨은 이미 다운로드되어 있습니다.");
        }
    }

    public async UniTask<bool> CheckForUpdates()
    {
        var checkHandle = Addressables.CheckForCatalogUpdates(false);
        var catalogsToUpdate = await checkHandle.ToUniTask();
        bool hasUpdates = catalogsToUpdate.Count > 0;

        Addressables.Release(checkHandle);
        return hasUpdates;
    }

    public async UniTask<long> GetTotalDownloadSize()
    {
        long totalSize = 0;

        foreach (AssetLabelReference label in _labelsToDownload)
        {
            var sizeHandle = Addressables.GetDownloadSizeAsync(label);
            long labelSize = await sizeHandle.ToUniTask();
            totalSize += labelSize;
            Addressables.Release(sizeHandle);
        }

        return totalSize;
    }

    private string FormatBytes(long bytes)
    {
        string[] suffixes = { "B", "KB", "MB", "GB", "TB" };
        int counter = 0;
        decimal number = (decimal)bytes;

        while (System.Math.Round(number / 1024) >= 1)
        {
            number = number / 1024;
            counter++;
        }

        return string.Format("{0:n1} {1}", number, suffixes[counter]);
    }

    // Inspector에서 테스트용
    [ContextMenu("Check for Downloads")]
    private async void TestCheckForDownloads()
    {
        await CheckForDownloads();
    }

    [ContextMenu("Start Download")]
    private async void TestStartDownload()
    {
        await StartDownload();
    }

    [ContextMenu("Get Download Size")]
    private async void TestGetDownloadSize()
    {
        long size = await GetTotalDownloadSize();
        Debug.Log($"전체 다운로드 크기: {FormatBytes(size)}");
    }
}