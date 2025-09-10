using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageUIController : MonoBehaviour
{
    [Header("MapSelectUI")]
    [SerializeField] Image _mapImage;
    [SerializeField] TMP_Text _mapNameText;

    private int _mapIndex = 0;

    private void Start()
    {
        MapUIUpdate(0);
    }

    public void MapUIUpdate(int index)
    {
        _mapIndex = index + 1;
        MapData data = TempDataManager.Instance.ReturnMapData(index);
        _mapImage.sprite = data.MapImage;
        _mapNameText.text = data.MapName;
    }
}