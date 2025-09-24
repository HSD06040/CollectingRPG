using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private static readonly int[] accelerates = { 1, 2, 4 };    
    public Property<float> CurrentAccelerate = new Property<float>();
    public Property<bool> IsPause = new Property<bool>();
    private int _currentIdx = 0;

    public void NextAccelerate()
    {
        _currentIdx = (_currentIdx + 1) % accelerates.Length;
        SetAccelerate(_currentIdx);
        CurrentAccelerate.Value = accelerates[_currentIdx];
    }

    public void ChangePause()
    {
        if(IsPause.Value)
        {
            IsPause.Value = false;
            SetAccelerate(_currentIdx);
        }
        else
        {
            IsPause.Value = true;
            SetTimeScale(0);
        }
    }

    private void SetAccelerate(int idx)
    {
        if (IsPause.Value)
            return;

        _currentIdx = idx;
        SetTimeScale(accelerates[_currentIdx]);
    }

    private void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = 0.02f * scale;
    }
}
