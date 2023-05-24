using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedStat : Stat
{
    public override void Init(Define.ChickenType type = Define.ChickenType.None, int level = 1)
    {
        if (Stats.Count > 0) Stats.Clear();

        Stats.Add(StatType.Hp, 10);
        Stats.Add(StatType.MoveSpeed, 0f);
        Stats.Add(StatType.AttackPower, 0);
        Stats.Add(StatType.EatRange, 0f);
        Stats.Add(StatType.Defense, 0);

        HpMax = (int)Stats[StatType.Hp];
    }
}