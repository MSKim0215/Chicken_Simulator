using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    public Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    // Ω∫≈» µ•¿Ã≈Õ
    public Dictionary<int, Data.Stat> StatDict { private set; get; } = new Dictionary<int, Data.Stat>();

    public void Init()
    {
        //StatDict = LoadJson<Data.StatData, int, Data.Stat>("StatData").MakeDict();
    }

    private Loader LoadJson<Loader, Key, Value>(string _path) where Loader: ILoader<Key,Value>
    {
        TextAsset file = Managers.Resource.Load<TextAsset>($"Data/{_path}");
        return JsonUtility.FromJson<Loader>(file.text);
    }
}