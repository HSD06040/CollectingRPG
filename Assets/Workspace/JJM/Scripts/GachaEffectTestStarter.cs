using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaEffectTestStarter : MonoBehaviour
{
    public GachaEffectController gachaEffectController;
    public UnitData testUnitData;

    

    void Start()
    {
        // 씬 시작 시 자동 테스트
        gachaEffectController.RequestGachaEffect(testUnitData);
        
    }
}
