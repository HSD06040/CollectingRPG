using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static bool Contain(this LayerMask layerMask, int layer)
    {
        return ((1 << layer) & layerMask) != 0;
    }

    public static int CalculateFinalDamage(int damage, int defense, DamageType damageType)
    {
        float totalDefense = defense / (defense + 100f);

        return Mathf.RoundToInt(damage * (1f - totalDefense));
    }

    public static int CalculateBaseDamage(this UnitStatusController status, float multiply, DamageType damageType)
    {
        int damage = damageType == DamageType.Physical ? status.PhysicalDamage.Value : status.MagicDamage.Value;
        float total = damage * multiply;

        if(status.CritChance.Value > Random.Range(0f, 100f))
        {
            total *= status.CritDamage.Value / 100;
        }

        return Mathf.RoundToInt(total);
    }

    private static Collider2D[] _hitBuffer = new Collider2D[50];
    private static readonly List<GameObject> _cachedTargets = new List<GameObject>(50);

    public static Transform GetClosestTargetNonAlloc(Vector3 origin, float radius, LayerMask enemyMask)
    {
        int count = Physics2D.OverlapCircleNonAlloc(origin, radius, _hitBuffer, enemyMask);

        Transform closest = null;
        float bestDistSq = float.PositiveInfinity;

        for (int i = 0; i < count; i++)
        {
            var hit = _hitBuffer[i];
            if (hit == null) continue;

            float distSq = (hit.transform.position - origin).sqrMagnitude;
            if (distSq < bestDistSq)
            {
                bestDistSq = distSq;
                closest = hit.transform;
            }
        }

        return closest;
    }

    public static GameObject[] GetTargetsNonAlloc(
        Vector2 origin,
        SearchType shape,
        float sizeOrRadius,
        Vector2 boxSize,
        float angle,
        int maxCount,        
        LayerMask layerMask,        
        System.Func<List<GameObject>, int, GameObject[]> filter = null,
        int maxTargets = 50,
        bool sortByDistance = true)
    {
        _cachedTargets.Clear(); // 재사용

        int hitCount = 0;

        switch (shape)
        {
            case SearchType.Circle:
                hitCount = Physics2D.OverlapCircleNonAlloc(origin, sizeOrRadius, _hitBuffer, layerMask);
                break;
            case SearchType.Box:
                hitCount = Physics2D.OverlapBoxNonAlloc(origin, boxSize, angle, _hitBuffer, layerMask);
                break;
            case SearchType.Capsule:
                hitCount = Physics2D.OverlapCapsuleNonAlloc(origin, boxSize, CapsuleDirection2D.Vertical, angle, _hitBuffer, layerMask);
                break;
        }

        for (int i = 0; i < hitCount && _cachedTargets.Count < maxCount; i++)
        {
            if (_hitBuffer[i] != null && _hitBuffer[i].gameObject != null)
                _cachedTargets.Add(_hitBuffer[i].gameObject);
        }        

        if (sortByDistance && _cachedTargets.Count > 1)
        {
            _cachedTargets.Sort((a, b) =>
            {
                Vector2 posA = a.transform.position;
                Vector2 posB = b.transform.position;
                float distA = (origin.x - posA.x) * (origin.x - posA.x) + (origin.y - posA.y) * (origin.y - posA.y);
                float distB = (origin.x - posB.x) * (origin.x - posB.x) + (origin.y - posB.y) * (origin.y - posB.y);
                return distA.CompareTo(distB);
            });
        }

        GameObject[] result;

        if(filter != null)
            result = filter.Invoke(_cachedTargets, maxTargets);
        else
            result = _cachedTargets.ToArray();

        return result;
    }

    public static int GetFacingDir(this Transform transform)
    {
        return transform.localScale.x > 0 ? -1 : 1;
    }
}
