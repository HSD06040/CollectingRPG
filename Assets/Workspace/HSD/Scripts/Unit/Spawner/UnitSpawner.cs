using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    private GameObject _unitPrefab;
    private float _damage;

    public void Init(GameObject unitPrefab, bool isAttack = false, float damage = 10)
    {
        _unitPrefab = unitPrefab;
        _damage = damage;
    }

    private void SpawnUnit()
    {
        Instantiate(_unitPrefab, transform.position, Quaternion.identity);
    }
}
