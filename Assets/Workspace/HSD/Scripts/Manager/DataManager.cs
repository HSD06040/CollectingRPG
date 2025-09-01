using Cysharp.Threading.Tasks;
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

        Manager.Resources.LoadLabel<UnitData>("UnitData").Forget();
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
