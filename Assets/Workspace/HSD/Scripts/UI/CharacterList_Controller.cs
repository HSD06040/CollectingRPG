using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterList_Controller : MonoBehaviour
{
    [SerializeField] GameObject _characterListPrefab;
    [SerializeField] Transform _content;
    private CharacterList_UI[] characterList_UIs;

    private void Start()
    {
        Setup();
    }

    public void Setup()
    {
        SynergyDatabase db = Manager.Data.SynergyDB;
        UnitData[] playerUnits = Manager.Data.PlayerUnitDatas;
        characterList_UIs = new CharacterList_UI[(int)Synergy.Length - ((int)ClassType.SUPPORT + 1)];

        int count = 0;
        for (int i = (int)Synergy.KINGDOM; i < (int)Synergy.Length; i++)
        {
            CharacterList_UI characterList_UI = Instantiate(_characterListPrefab, _content).GetComponent<CharacterList_UI>();
            SynergyData synergy = db.GetSynergy(i);
            characterList_UI.Setup(synergy.SynergyName, System.Array.FindAll(playerUnits, unit => unit.Synergy == (Synergy)i));
            characterList_UIs[count] = characterList_UI;
            count++;
        }
    }
}
