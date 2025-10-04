using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

public class AddressablesLabelTool
{
    [MenuItem("Collecting_RPG/Addressables/Add Label To All Assets")]
    public static void AddLabelToAllAssets()
    {
        string labelName = "DefaultDownloadAssets"; // 자동으로 붙일 라벨 이름
        
        var settings = AddressableAssetSettingsDefaultObject.Settings;

        if (settings == null)
        {
            Debug.LogError("AddressableAssetSettings을 찾을 수 없습니다. Addressables이 세팅되어 있는지 확인하세요.");
            return;
        }

        // 라벨 없으면 추가
        if (!settings.GetLabels().Contains(labelName))
        {
            settings.AddLabel(labelName);
            Debug.Log($"새 라벨 생성: {labelName}");
        }
        
        int count = 0;
        foreach (var group in settings.groups)
        {
            if (group == null) continue;

            foreach (var entry in group.entries)
            {
                if (!entry.labels.Contains(labelName))
                {
                    entry.SetLabel(labelName, true, true);
                    count++;
                }
            }
        }
        
        AssetDatabase.SaveAssets();
        Debug.Log($"모든 Addressables 에셋에 라벨 '{labelName}' 추가 완료 (총 {count}개 적용).");
    }
}
