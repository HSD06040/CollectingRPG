using Cysharp.Threading.Tasks;
using System.Diagnostics;
using UnityEngine.AddressableAssets;

public class DataManager : Singleton<DataManager>
{
    public UnitData[] UnitDatas;

    private void Awake()
    {
        InitData().Forget();
    }

    public async UniTask InitData()
    {
        CsvLoadData data;

        data = await Addressables.LoadAssetAsync<CsvLoadData>("Data/CsvLoadData");         

        CsvDownloader csvDownloader = new CsvDownloader(data);

        await PreLoadData();

        csvDownloader.DownloadDataAsync().Forget();
    }

    private async UniTask PreLoadData()
    {
        await PreLoadUnitDatas();
    }

    private async UniTask PreLoadUnitDatas()
    {
        UnitDatas = await Manager.Resources.LoadAll<UnitData>("UnitData");
    }
}
