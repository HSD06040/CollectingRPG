using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    private Vector2 _minAnchor;
    private Vector2 _maxAnchor;

    private void Start()
    {
        var myRect = this.GetComponent<RectTransform>();

        _minAnchor = Screen.safeArea.min;
        _maxAnchor = Screen.safeArea.max;

        _minAnchor.x /= Screen.width;
        _minAnchor.y /= Screen.height;
        _maxAnchor.x /= Screen.width;
        _maxAnchor.y /= Screen.height;

        myRect.anchorMin = _minAnchor;
        myRect.anchorMax = _maxAnchor;
    }
}
