using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ResourcesManager : Singleton<ResourcesManager>
{
    private static Dictionary<string, Object> resources = new Dictionary<string, Object>();

    public async UniTask LoadLabel<T>(string label) where T : Object
    {
        var locationsHandle = Addressables.LoadResourceLocationsAsync(label, typeof(T));
        var locations = await locationsHandle.Task;

        foreach (var location in locations)
        {
            var handle = Addressables.LoadAssetAsync<T>(location);
            var asset = await handle.Task;

            if (!resources.ContainsKey(location.PrimaryKey))
                resources.Add(location.PrimaryKey, asset);
        }

        Addressables.Release(locationsHandle);
    }

    public async UniTask<T[]> LoadAll<T>(string label) where T : Object
    {
        var handle = Addressables.LoadAssetsAsync<T>(label, null);
        var result = await handle.Task;

        return result.ToArray();
    }

    public void Unload(string path)
    {
        if (resources.ContainsKey(path))
        {
            Resources.UnloadAsset(resources[path]);
            resources.Remove(path);
        }
    }

    public void UnloadAll()
    {
        
    }

    public T Load<T> (string path) where T : Object
    {
        return resources.ContainsKey(path) ? resources[path] as T : null;
    }

    public T Instantiate<T>(T original, Vector3 position, Quaternion rotation, Transform parent, bool isPool = false) where T : Object
    {
        GameObject obj = original as GameObject;

        if (isPool)
            return Manager.Pool.Get(obj, position, rotation, parent) as T;
        else
            return Object.Instantiate(obj, position, rotation, parent) as T;
    }

    public T Instantiate<T>(T original, Vector3 position, Quaternion rotation, bool isPool = false) where T : Object
    {
        return Instantiate(original, position, rotation, null, isPool);
    }

    public T Instantiate<T>(T original, Vector3 position, bool isPool = false) where T : Object
    {
        return Instantiate(original, position, Quaternion.identity, null, isPool);
    }

    public T Instantiate<T>(string path, Vector3 position, Quaternion rotation, Transform parent, bool isPool = false) where T : Object
    {
        T obj = Load<T>(path);
        return Instantiate(obj, position, rotation, parent, isPool);
    }

    public T Instantiate<T>(string path, Vector3 position, Quaternion rotation, bool isPool = false) where T : Object
    {
        return Instantiate<T>(path, position, rotation, null, isPool);
    }

    public T Instantiate<T>(string path, Vector3 postion, bool isPool = false) where T : Object
    {
        return Instantiate<T>(path, postion, Quaternion.identity, null, isPool);
    }

    public void Destroy(GameObject obj)
    {
        if (obj == null || !obj.activeSelf) return;

        if (Manager.Pool.ContainsKey(obj.name))
            Manager.Pool.Release(obj);
        else
            Object.Destroy(obj);
    }

    public void Destroy(GameObject obj, float delay)
    {
        if (Manager.Pool.ContainsKey(obj.name))
            Manager.Pool.Release(obj, delay);
        else
            Object.Destroy(obj, delay);
    }
}
