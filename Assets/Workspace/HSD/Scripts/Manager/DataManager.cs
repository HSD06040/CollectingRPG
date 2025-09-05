using Cysharp.Threading.Tasks;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

public class DataManager : Singleton<DataManager>
{
    public UnitData[] UnitDatas;
    public SynergyDatabase SynergyDB;

    private void Awake()
    {
        InitData().Forget();
    }

    public async UniTask InitData()
    {
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
        UnitDatas = await Manager.Resources.LoadAll<UnitData>("UnitData");
    }

    private async UniTask PreLoadSynergyDB()
    {
        SynergyDB = await Addressables.LoadAssetAsync<SynergyDatabase>("Database/SynergyDatabase");
        SynergyDB.Init();
    }
}
