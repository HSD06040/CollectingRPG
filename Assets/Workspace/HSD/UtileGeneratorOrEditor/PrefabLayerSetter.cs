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
        }
    }
}
