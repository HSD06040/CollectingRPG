using Unity.VisualScripting;
using UnityEngine;

public class PrefabLayerSetter : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private string targetLayerName;

    [ContextMenu("Apply Layer To Prefabs")]
    public void ApplyLayerToPrefabs()
    {
        foreach (var prefab in prefabs)
        {
            if (prefab == null) continue;

            SpriteRenderer[] renderers = prefab.GetComponentsInChildren<SpriteRenderer>(true);

            foreach (var renderer in renderers)
            {
                renderer.sortingLayerName = targetLayerName;
            }

            Debug.Log($"{prefab.name} 안의 SpriteRenderer {renderers.Length}개 레이어를 '{targetLayerName}'(으)로 변경 완료.");

            prefab.layer = LayerMask.NameToLayer("Player");
            prefab.tag = "Unit";

            if(!prefab.TryGetComponent<UnitBase>(out var ub))
                prefab.AddComponent<UnitBase>();

            if(!prefab.TryGetComponent<UnitStatusController>(out var usc))
                prefab.AddComponent<UnitStatusController>();

            if(!prefab.TryGetComponent<CapsuleCollider2D>(out var col))
                col = prefab.AddComponent<CapsuleCollider2D>();

            col.size = new Vector2(0.5f, .8f);
            col.offset = new Vector2(0, prefab.transform.localScale.y / 3);
            col.isTrigger = true;

            if(prefab.GetComponent<Rigidbody2D>() == null)
                prefab.AddComponent<Rigidbody2D>().gravityScale = 0;

            Transform child = prefab.transform.GetChild(0);
            child.tag = "UnitTrigger";

            if(child.TryGetComponent<BaseFSM>(out var fsm))
                fsm = child.AddComponent<BaseFSM>();

            fsm.Owner = prefab.GetComponent<UnitBase>();

            if(child.TryGetComponent<BoxCollider2D>(out var boxCol))
                boxCol = child.AddComponent<BoxCollider2D>();

            boxCol.isTrigger = true;
            boxCol.offset = new Vector2(0, prefab.transform.localScale.y / 3);

            prefab.GetComponent<UnitBase>().Awake();
        }
    }
}
