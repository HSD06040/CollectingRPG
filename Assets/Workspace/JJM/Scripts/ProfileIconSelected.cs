using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileIconSelected : MonoBehaviour
{
    public GameObject iconSelectPanel;

    public void OnProfileIconClick()
    {
        iconSelectPanel.SetActive(true);
    }
}
