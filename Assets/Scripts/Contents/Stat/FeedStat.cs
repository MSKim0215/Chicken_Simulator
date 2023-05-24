using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedStat : Stat
{
    private void Start()
    {
        Stats.Add(StatType.Hp, 10);
        Stats.Add(StatType.MoveSpeed, 0f);
        Stats.Add(StatType.AttackPower, 0);
        Stats.Add(StatType.EatRange, 0f);
        Stats.Add(StatType.Defense, 0);

        HpMax = (int)Stats[StatType.Hp];
    }
}