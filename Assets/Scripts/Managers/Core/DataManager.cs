using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    public Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, Data.ChickenStat> ChickStatDict { private set; get; } = new Dictionary<int, Data.ChickenStat>();
    public Dictionary<int, Data.ChickenStat> ChickenStatDict { private set; get; } = new Dictionary<int, Data.ChickenStat>();
    public Dictionary<int, Data.ChickenStat> EggStatDict { private set; get; } = new Dictionary<int, Data.ChickenStat>();

    public void Init()
    {
        ChickStatDict = LoadJson<Data.ChickStatData, int, Data.ChickenStat>("UnitData").MakeDict();
        ChickenStatDict = LoadJson<Data.ChickenStatData, int, Data.ChickenStat>("UnitData").MakeDict();
        EggStatDict = LoadJson<Data.EggStatData, int, Data.ChickenStat>("UnitData").MakeDict();
    }

    private Loader LoadJson<Loader, Key, Value>(string _path) where Loader: ILoader<Key,Value>
    {
        TextAsset file = Managers.Resource.Load<TextAsset>($"Data/{_path}");
        return JsonUtility.FromJson<Loader>(file.text);
    }
}