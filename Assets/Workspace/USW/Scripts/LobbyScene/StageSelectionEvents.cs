using System;

public static class StageSelectionEvents
{
    public static event Action<int, int> OnStageSelected; 
    
    public static void SelectStage(int region, int stage)
    {
        OnStageSelected?.Invoke(region, stage);
    }
}