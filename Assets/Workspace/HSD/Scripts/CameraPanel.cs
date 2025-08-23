using UnityEngine.EventSystems;
using UnityEngine;

public class CameraPanel : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] BattleCameraController _cameraController;
    [SerializeField] BaseCameraController _baseCameraController;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _cameraController.StartDrag();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // eventData.delta를 카메라 컨트롤러에 전달
        _cameraController.CameraUpdate(eventData.delta);
        _baseCameraController.OnDrag(-eventData.delta.y);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _cameraController.EndDrag();
        _baseCameraController.OnDragEnd();
    }
}