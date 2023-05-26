using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    public Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, Data.BaseChickenLevelTable> ChickenGroupLevelTable { private set; get; } = new Dictionary<int, Data.BaseChickenLevelTable>();
    public Dictionary<int, Data.BaseChickenStat> ChickenGroupStat { private set; get; } = new Dictionary<int, Data.BaseChickenStat>();

    public void Init()
    {
        ChickenGroupLevelTable = LoadJson<Data.ChickenLevelTableData, int, Data.BaseChickenLevelTable>("UnitData").MakeDict();
        ChickenGroupStat = LoadJson<Data.ChickenStatData, int, Data.BaseChickenStat>("UnitData").MakeDict();
    }

    private Loader LoadJson<Loader, Key, Value>(string _path) where Loader: ILoader<Key,Value>
    {
        TextAsset file = Managers.Resource.Load<TextAsset>($"Data/{_path}");
        return JsonUtility.FromJson<Loader>(file.text);
    }
}