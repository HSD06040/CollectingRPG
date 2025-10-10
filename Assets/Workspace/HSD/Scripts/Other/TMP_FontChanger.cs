#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;

public class TMP_FontChanger : MonoBehaviour
{
    [SerializeField] TMP_FontAsset fontAsset;
    [SerializeField] string path;

    [ContextMenu("Change")]
    public void ChangeFont()
    {
        var texts = FindObjectsOfType<TMP_Text>(true);

        foreach (var text in texts)
        {
            text.font = fontAsset;            
        }
    }
    [ContextMenu("Change Font In Prefabs")]
    public void ChangeFontInPrefabs()
    {
        if (fontAsset == null)
        {
            Debug.LogError("FontAsset이 지정되지 않았습니다!");
            return;
        }

        if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
        {
            Debug.LogError($"경로가 잘못되었습니다: {path}");
            return;
        }

        string[] prefabGUIDs = AssetDatabase.FindAssets("t:Prefab", new[] { path });
        int changedCount = 0;

        foreach (string guid in prefabGUIDs)
        {
            string prefabPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            if (prefab == null)
                continue;

            bool modified = false;
            var texts = prefab.GetComponentsInChildren<TMP_Text>(true);
            foreach (var text in texts)
            {
                if (text.font != fontAsset)
                {
                    text.font = fontAsset;
                    modified = true;
                }
            }

            if (modified)
            {
                EditorUtility.SetDirty(prefab);
                PrefabUtility.SavePrefabAsset(prefab);
                changedCount++;
                Debug.Log($"폰트 변경됨: {prefabPath}");
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"완료! {changedCount}개의 프리팹에서 폰트가 변경되었습니다.");
    }
}
#endif