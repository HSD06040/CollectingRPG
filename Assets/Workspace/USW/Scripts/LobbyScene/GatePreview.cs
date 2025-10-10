using UnityEngine;
using UnityEngine.UI;
public class GatePreview : MonoBehaviour
{
    [SerializeField] Image _gatePreviewImage;

    private void OnEnable()
    {
        StageSelectionEvents.OnStageSelected += UpdatePreview;
    }

    private void OnDisable()
    {
        StageSelectionEvents.OnStageSelected -= UpdatePreview;
    }

    private void UpdatePreview(int region, int stage)
    {
        StageData stageData = Manager.Data.StageDatas.GetStage(region);

        if (stageData != null && _gatePreviewImage != null)
        {
            _gatePreviewImage.sprite = stageData.RegionPreviewSprite;
            _gatePreviewImage.enabled = true;
        }
    }
}