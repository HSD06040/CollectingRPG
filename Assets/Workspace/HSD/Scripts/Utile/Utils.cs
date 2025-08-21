using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public static class Utils
{
    private static GameObject _damagePopUpObj;
    private static GameObject _worldCanvas;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        _damagePopUpObj = Addressables.LoadAssetAsync<GameObject>("DamagePopUp").WaitForCompletion();
        GameObject obj = Addressables.LoadAssetAsync<GameObject>("WorldCanvas").WaitForCompletion();
        _worldCanvas = Object.Instantiate(obj);
    }

    public static bool Contain(this LayerMask layerMask, int layer)
    {
        return ((1 << layer) & layerMask) != 0;
    }   

    public static void CalculateDamage(this UnitStatusController status, float attackPower, DamageType damageType, UnitStatusController enemy)
    {
        int damage = damageType == DamageType.Physical ? status.PhysicalDamage.Value : status.MagicDamage.Value;
        int defense = damageType == DamageType.Physical ? enemy.PhysicalDefense.Value : enemy.MagicDefense.Value;
        float total = damage * attackPower;

        bool isCrit = false;

        if (status.CritChance.Value > Random.Range(0f, 100f))
        {
            isCrit = true;
            total *= status.CritDamage.Value / 100;
        }

        float totalDefense = defense / (defense + 100f);

        int totalDamage = Mathf.RoundToInt(total * (1f - totalDefense));

        Object.Instantiate(_damagePopUpObj, enemy.transform.position, Quaternion.identity, _worldCanvas.transform).
            GetComponent<DamagePopUp>().Init(totalDamage, isCrit);

        status.TotalDamage.Value += totalDamage;

        enemy.TakeDamage(totalDamage);
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

    #region GetTargetsNonAlloc
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

        if (filter != null)
            result = filter.Invoke(_cachedTargets, maxTargets);
        else
            result = _cachedTargets.ToArray();

        return result;
    }

    public static GameObject GetTargetsNonAllocSingle(
        IAttacker attacker,
        SearchType shape,
        float sizeOrRadius,
        Vector2 boxSize,
        float angle,
        LayerMask layerMask,
        System.Func<IAttacker, List<GameObject>, GameObject> filter = null)
    {
        _cachedTargets.Clear(); // 재사용

        int hitCount = 0;

        Vector2 origin = attacker.GetTransform().position;

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

        GameObject result;

        if (filter != null)
            result = filter.Invoke(attacker, _cachedTargets);
        else
            result = _cachedTargets[0];

        return result;
    }
    #endregion

    public static int GetFacingDir(this Transform transform)
    {
        return transform.localScale.x > 0 ? -1 : 1;
    }

    public static string ToAbbreviation(long value)
    {
        if (value >= 1_000_000_000)
            return $"{(value / 1_000_000_000f).ToString("0.#")}B";
        if (value >= 1_000_000)
            return $"{(value / 1_000_000f).ToString("0.#")}M";
        if (value >= 1_000)
            return $"{(value / 1_000f).ToString("0.#")}k";

        return value.ToString();
    }
}
