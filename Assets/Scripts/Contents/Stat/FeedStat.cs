using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedStat : BaseStat
{
    public void Init(Define.ChickenType type = Define.ChickenType.None, int level = 1)
    {
        if (GenesStats.Count > 0) GenesStats.Clear();

        GenesStats.Add(GenesStatType.HpMax, 10);
        GenesStats.Add(GenesStatType.MoveSpeed, 0f);
        GenesStats.Add(GenesStatType.EatPower, 0);
        GenesStats.Add(GenesStatType.EatRange, 0f);
        GenesStats.Add(GenesStatType.Defense, 0);

        NowHp = (int)GenesStats[GenesStatType.HpMax];
    }
}