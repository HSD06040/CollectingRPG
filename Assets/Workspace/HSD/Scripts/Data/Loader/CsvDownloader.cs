using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CsvDownloader : MonoBehaviour
{
    private const string URL = "";

    public event Action OnDataSetupCompleted;
    /// <summary>
    /// 데이터 다운로드 및 세팅
    /// </summary>
    public async UniTask DownloadDataAsync()
    {        
        List<UniTask> task = new List<UniTask>
        {
            LoadCSV(URL, SetupTest, 4),         
        };

        await UniTask.WhenAll(task);

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

    // 예시 세팅 함수를 각자 다르게 구현
    private void SetupTest(string[][] data)
    {
        //foreach (string[] row in data)
        //{
        //    int ID = int.Parse(row[0]);

        //    MonsterStat stat = Array.Find(Manager.Table.monsterStat, m => m.ID == ID);

        //    if (stat != null)
        //    {
        //        stat.ID = ID;
        //        stat.monsterName = row[1];
        //        stat.health = float.Parse(row[2]);
        //        stat.attackPower = int.Parse(row[3]);
        //        stat.moveSpeed = float.Parse(row[4]);
        //        stat.GetCoinAmount = int.Parse(row[5]);
        //    }
        //}
    }
}
