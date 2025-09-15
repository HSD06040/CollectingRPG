using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DataManager : Singleton<DataManager>
{
    public Dictionary<string, UnitData> UnitDataDic;
    public Dictionary<string, RuntimeAnimatorController> Animators;
    public UnitData[] EnemyUnitDatas;
    public SynergyDatabase SynergyDB;

    private void Awake()
    {
        InitData().Forget();
    }

    public async UniTask InitData()
    {
        AnimatorSetting().Forget();
        await PreLoadData();
        await CsvDownload();
    }

    private async UniTask CsvDownload()
    {
        CsvLoadData data = await Addressables.LoadAssetAsync<CsvLoadData>("Data/CsvLoadData");
        CsvDownloader csvDownloader = new CsvDownloader(data);

        csvDownloader.DownloadDataAsync().Forget();
    }

    private async UniTask PreLoadData()
    {
        UniTask[] tasks = new UniTask[2];

        tasks[0] = PreLoadSynergyDB();
        tasks[1] = PreLoadUnitDatas();

        await UniTask.WhenAll(tasks);
    }

    private async UniTask PreLoadUnitDatas()
    {
        EnemyUnitDatas = await Manager.Resources.LoadAll<UnitData>("EnemyUnitData");
        UnitData[] UnitDatas = await Manager.Resources.LoadAll<UnitData>("UnitData");

        UnitDataDic = new Dictionary<string, UnitData>(UnitDatas.Length);

        foreach (var unitData in UnitDatas)
        {
            if (!UnitDataDic.ContainsKey(unitData.Name))
                UnitDataDic.Add(unitData.Name, unitData);

            unitData.Init();
        }
    }

    private async UniTask PreLoadSynergyDB()
    {
        SynergyDB = await Addressables.LoadAssetAsync<SynergyDatabase>("Database/SynergyDatabase");
        SynergyDB.Init();
    }

    private async UniTask AnimatorSetting()
    {
        UnitAnimatorData animatorData = await Addressables.LoadAssetAsync<UnitAnimatorData>("UnitAnimatorData");

        foreach (var data in animatorData.Animators)
        {
            AnimatorOverrideController newAnimator = new AnimatorOverrideController(animatorData.BaseController);

            newAnimator["Melee_Attack"] = data.AttackAnimationClip;
            newAnimator["Melee_Skill"] = data.SkillAnimationClip;

            Animators.Add(data.AnimatorName, newAnimator.runtimeAnimatorController);
        }
    }

    public UnitData GetUnitData(string unitName)
    {
        return UnitDataDic.TryGetValue(unitName, out var unitData) ? unitData : null;
    }
}
