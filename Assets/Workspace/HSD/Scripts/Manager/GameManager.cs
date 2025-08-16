using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private const float DEFAULT_ACCELERATE = 1.0f;
    public float CurrentAccelerate;

    private void Awake()
    {
        SetAccelerate(DEFAULT_ACCELERATE);
    }

    public void SetAccelerate(float accelerate)
    {
        Time.timeScale = accelerate;
        Time.fixedDeltaTime = 0.02f * accelerate; // Adjust fixed delta time based on the time scale
        CurrentAccelerate = accelerate;
    }
}
