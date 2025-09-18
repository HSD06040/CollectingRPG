using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TestAddressableLoad : MonoBehaviour
{
    private void Start()
    {
        Manager.Resources.LoadLabel("Test").Forget();        
    }
}
