using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AddressableTest : MonoBehaviour
{
    [SerializeField] AssetLabelReference labelReference;
    [SerializeField] string adress;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Manager.Resources.UnloadLabel(labelReference);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Manager.Resources.Instantiate<GameObject>(adress, Vector2.zero);
        }
    }
}
