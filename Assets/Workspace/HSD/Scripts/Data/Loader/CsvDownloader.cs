using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CsvDownloader
{
    private CsvLoadData _csvLoadData;

    public event Action OnDataSetupCompleted;

    public CsvDownloader(CsvLoadData csvLoadData)
    {
        _csvLoadData = csvLoadData;
    }

    /// <summary>
    /// 데이터 다운로드 및 세팅
    /// </summary>
    public async UniTask DownloadDataAsync()
    {
        List<UniTask> tasks = new List<UniTask>(10);

        foreach (var csvData in _csvLoadData.CsvDatas)
        {
            string url = csvData.URL;
            tasks.Add(LoadCSV(url, GetSetupMethod(csvData.CsvType), csvData.StartLine));         
        }

        await UniTask.WhenAll(tasks);

        Debug.Log("끝!");
        
        OnDataSetupCompleted?.Invoke();
    }

    /// <summary>
    /// CSV 다운로드 + 파싱
    /// </summary>
    private async UniTask LoadCSV(string url, Action<string[][]> onParsed, int startLine = 1)
    {
        using UnityWebRequest req = UnityWebRequest.Get(url);

        await req.SendWebRequest().ToUniTask();

        if (!string.IsNullOrEmpty(req.error))
        {
            Debug.LogError($"CSV 다운로드 실패: {url}, Error: {req.error}");
            return;
        }

        string raw = req.downloadHandler.text.Trim();
        string[] lines = raw.Split('\n');
        List<string[]> parsed = new();

        for (int i = startLine - 1; i < lines.Length; i++)
        {
            string[] row = lines[i].Trim().Split(',');
            parsed.Add(row);
        }

        onParsed?.Invoke(parsed.ToArray());
    }

    private Action<string[][]> GetSetupMethod(CsvType csvType)
    {
        switch(csvType)
        {
            case CsvType.UnitStat:
                return UnitStatSetup;
            default:
                Debug.LogError($"알 수 없는 CSV 이름: {csvType.ToString()}");
                return null;
        }
    }

    private void UnitStatSetup(string[][] data)
    {
        foreach (var row in data)
        {
            
        }
    }
}
