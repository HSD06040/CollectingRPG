using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

public class BadWordCSV
{
    private string _url;

    public BadWordCSV(string url)
    {
        _url = url;
    }

    /// <summary>
    /// 비속어 CSV를 로드하고 파싱하는 메서드
    /// </summary>
    public async UniTask<List<string>> LoadAsync(int startLine = 1)
    {
        using UnityWebRequest request = UnityWebRequest.Get(_url);
        await request.SendWebRequest().ToUniTask();

        if (!string.IsNullOrEmpty(request.error))
        {
            Debug.LogError($"비속어 CSV 다운로드 실패: {request.error}");
            return null;
        }

        string raw = request.downloadHandler.text.Trim();
        string[] lines = raw.Split('\n');

        List<string> words = new();

        for (int i = startLine - 1; i < lines.Length; i++)
        {
            string word = lines[i].Trim();

            if (!string.IsNullOrEmpty(word))
                words.Add(word);
        }

        return words;
    }
}