using Cysharp.Threading.Tasks;
using Map;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DataManager : Singleton<DataManager>
{
    // 유닛 데이터 관련
    public Dictionary<string, UnitData> UnitDataDic;    
    public UnitData[] EnemyUnitDatas;
    public UnitData[] PlayerUnitDatas;

    // 시너지
    public SynergyDatabase SynergyDB;

    // 스테이지 관련
    public StageGameData StageGameData = new();
    public StageDatas StageDatas = new();

    // 인게임
    public UnitSpawnChanceData UnitSpawnChanceData;
    public CharacterSellAmountData CharacterSellAmountData;
    public AugmentChanceData AugmentChanceData;
    public PriceDatas PriceDatas;
    public AUGData[] AugmentDatas;

    // 애니메이션 데이터
    public AnimationManager AnimationManager = new();

    // 마법석 데이터 임시로 추가
    public Dictionary<string, MagicStone> MagicStoneDic;
    public MagicStoneData[] MagicStoneDatas;
    public MagicStone[] MagicStones;
    public MagicStonLevelChanceData MagicStoneLevelChanceData;

    // 프리셋 데이터 관련
    public PresetDatabase PresetDB { get; private set; } = new PresetDatabase();

    // 맵 데이터 관련
    public MapDatabase MapDB { get; private set; } = new MapDatabase();
    public StageGridData StageGridData = new();
    public EventData[] EventDatas;

    public async UniTask InitAsync()
    {
        StageDatas.Init().Forget();
        PresetDB.InitPresetData();
        MapDB.InitMapData();
        await InitData();
    }

    public async UniTask InitData()
    {
        await SpritesLoad();

        UnitSpawnChanceData = await Addressables.LoadAssetAsync<UnitSpawnChanceData>("Data/UnitSpawnChanceData");
        CharacterSellAmountData = await Addressables.LoadAssetAsync<CharacterSellAmountData>("Data/CharacterSellAmountData");
        MagicStoneLevelChanceData = await Addressables.LoadAssetAsync<MagicStonLevelChanceData>("Data/MagicStoneLevelChanceData");        

        await AnimationManager.Init();
        await PreLoadData();
        await CsvDownload();
        //PreLoadMagicStoneDatas();
    }

    private static async UniTask SpritesLoad()
    {
        await Manager.Resources.SpriteLoadLable("MonsterIcon");
        await Manager.Resources.SpriteLoadLable("PlayerUnitIcon");
        await Manager.Resources.SpriteLoadLable("SkillIcon");
        await Manager.Resources.SpriteLoadLable("LobbySprite");
    }

    private static async UniTask CsvDownload()
    {
        CsvLoadData data = await Addressables.LoadAssetAsync<CsvLoadData>("Data/CsvLoadData");
        CsvDownloader csvDownloader = new CsvDownloader(data);

        csvDownloader.DownloadDataAsync().Forget();
    }

    public UnitData GetUnitData(string unitName)
    {
        return UnitDataDic.TryGetValue(unitName, out var unitData) ? unitData : null;
    }

    #region PreLoadData
    private async UniTask PreLoadData()
    {
        List<UniTask> tasks = new List<UniTask>();

        tasks.Add(PreLoadSynergyDB());
        tasks.Add(PreLoadUnitDatas());
        tasks.Add(PreLoadMagicStoneDatas());
        tasks.Add(PreLoadAugmentDatas());
        tasks.Add(PreLoadEventDatas());

        await UniTask.WhenAll(tasks);
    }        

    private async UniTask PreLoadEventDatas()
    {
        EventDatas = await Manager.Resources.LoadAll<EventData>("EventData");
    }

    private async UniTask PreLoadAugmentDatas()
    {
        AugmentDatas = await Manager.Resources.LoadAll<AUGData>("AugmentData");
        AugmentChanceData = await Addressables.LoadAssetAsync<AugmentChanceData>("Data/AugmentChanceData");
        PriceDatas = await Addressables.LoadAssetAsync<PriceDatas>("Data/PriceDatas");
    }

    private async UniTask PreLoadUnitDatas()
    {
        EnemyUnitDatas = await Manager.Resources.LoadAll<UnitData>("EnemyUnitData");        
        PlayerUnitDatas = await Manager.Resources.LoadAll<UnitData>("UnitData");

        UnitDataDic = new Dictionary<string, UnitData>(PlayerUnitDatas.Length);

        foreach (var unitData in PlayerUnitDatas)
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

    #region MagicStone

    //private void PreLoadMagicStoneDatas()
    //{
    //    MagicStoneDataDic = new Dictionary<string, MagicStoneData>(MagicStoneDatas.Length);

    //    foreach (var magicStoneData in MagicStoneDatas)
    //    {
    //        if (!MagicStoneDataDic.ContainsKey(magicStoneData.Name))
    //            MagicStoneDataDic.Add(magicStoneData.Name, magicStoneData);

    //        magicStoneData.Init();
    //    }
    //}

    private async UniTask PreLoadMagicStoneDatas()
    {
        MagicStones = await Manager.Resources.LoadAll<MagicStone>("MagicStone");

        MagicStoneDic = new Dictionary<string, MagicStone>();

        foreach (var magicStone in MagicStones)
        {
            if (!MagicStoneDic.ContainsKey(magicStone.Name))
                MagicStoneDic.Add(magicStone.Name, magicStone);

            magicStone.Init();
        }
    }

    public MagicStone GetMagicStoneData(string magicStoneName)
    {
        if(!MagicStoneDic.ContainsKey(magicStoneName))
        {
            foreach (var data in MagicStones)
            {
                if(!MagicStoneDic.ContainsKey(data.Name))
                    MagicStoneDic.Add(data.Name, data);
            }
        }

        return MagicStoneDic.TryGetValue(magicStoneName, out var magicStoneData) ? magicStoneData : null;
    }
    public MagicStoneData GetRandomMagicStoneData()
    {
        if (MagicStoneDatas.Length == 0)
            return null;

        return MagicStoneDatas[Random.Range(0, MagicStoneDatas.Length)];
    }
    #endregion

#endregion
}
