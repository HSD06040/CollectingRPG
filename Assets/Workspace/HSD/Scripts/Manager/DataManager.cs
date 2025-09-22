using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static UnitAnimatorData;

public class DataManager : Singleton<DataManager>
{
    // 유닛 데이터 관련
    public Dictionary<string, UnitData> UnitDataDic;
    public Dictionary<AnimatorData, RuntimeAnimatorController> AnimatorDic;
    public UnitData[] EnemyUnitDatas;
    public UnitData[] UnitDatas;
    public SynergyDatabase SynergyDB;

    // 프리셋 데이터 관련
    public PresetDatabase PresetDB { get; private set; } = new PresetDatabase();

    // 맵 데이터 관련
    public MapDatabase MapDB { get; private set; } = new MapDatabase();

    // 추후 Init으로 뺄 예정
    private void Awake()
    {
        InitData().Forget();
        PresetDB.InitPresetData();
        MapDB.InitMapData();
    }

    #region UniData

    public async UniTask InitData()
    {
        await Manager.Resources.SpriteLoadLable("MonsterIcon");
        await AnimatorSetting();
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
        UnitDatas = await Manager.Resources.LoadAll<UnitData>("UnitData");

        UnitDataDic = new Dictionary<string, UnitData>(UnitDatas.Length);

        foreach (var unitData in UnitDatas)
        {
            if (!UnitDataDic.ContainsKey(unitData.Name))
                UnitDataDic.Add(unitData.Name, unitData);

            unitData.Init();
        }

        foreach (var unitData in EnemyUnitDatas)
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
        UnitAnimatorData animatorData = await Addressables.LoadAssetAsync<UnitAnimatorData>("Data/UnitAnimatorData");

        AnimatorDic = new Dictionary<AnimatorData, RuntimeAnimatorController>(animatorData.Animators.Length);

        animatorData.SettingAnimationClip();

        foreach (var data in animatorData.Animators)
        {
            AnimatorOverrideController newAnimator = new AnimatorOverrideController(animatorData.BaseController);

            foreach (var pair in newAnimator.animationClips)
            {
                if (pair.name == "Melee_Attack")
                    newAnimator[pair.name] = animatorData.GetAnimationClip(data.AttackAnimationType);
                else if (pair.name == "Melee_Skill")
                    newAnimator[pair.name] = animatorData.GetAnimationClip(data.SkillAnimationType);
            }

            if (!AnimatorDic.ContainsKey(data))
                AnimatorDic.Add(data, newAnimator.runtimeAnimatorController);
        }
    }

    public UnitData GetUnitData(string unitName)
    {
        return UnitDataDic.TryGetValue(unitName, out var unitData) ? unitData : null;
    }

    #endregion
}
