using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchButtonController : MonoBehaviour
{
    [SerializeField] VerticalSwipePager _verticalSwipePager;

    [Header("Buttons")]
    [SerializeField] Button _battleSwitchButton;
    [SerializeField] Button _unitSwitchButton;

    private void Start()
    {
        _battleSwitchButton.onClick.AddListener(() => _verticalSwipePager.MoveToPage(1));

        _unitSwitchButton.onClick.AddListener(() => _verticalSwipePager.MoveToPage(0));
    }
}
