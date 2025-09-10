using TMPro;
using UnityEngine;

public class StageUIController : MonoBehaviour
{
    [SerializeField] TMP_Text _mapNameText;

    private int _mapIndex = 0;

    private void Awake()
    {
        MapUIUpdate(0);
    }

    // 테스트용 코드
    public void MapUIUpdate(int index)
    {
        _mapIndex = index + 1;
        _mapNameText.text = $"지역이미지{_mapIndex}";
    }
}
