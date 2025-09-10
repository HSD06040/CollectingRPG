using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private static readonly int[] accelerates = { 1, 2, 4 };    
    public Property<float> CurrentAccelerate = new Property<float>();
    private int _currentIdx = 0;

    public void NextAccelerate()
    {
        _currentIdx = (_currentIdx + 1) % accelerates.Length;
        SetAccelerate(_currentIdx);
        CurrentAccelerate.Value = accelerates[_currentIdx];
    }

    private void SetAccelerate(int idx)
    {
        _currentIdx = idx;
        Time.timeScale = accelerates[_currentIdx];
        Time.fixedDeltaTime = 0.02f * accelerates[_currentIdx];
    }
}
