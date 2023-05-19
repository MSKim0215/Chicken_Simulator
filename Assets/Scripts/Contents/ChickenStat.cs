using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenStat : Stat
{
    private void Start()
    {
        Stats.Add(StatType.Hp, 5);
        Stats.Add(StatType.MoveSpeed, 1f);
        Stats.Add(StatType.AttackPower, 1);
        Stats.Add(StatType.EatRange, 0.13f);
        Stats.Add(StatType.Defense, 0);

        HpMax = (int)Stats[StatType.Hp];
    }
}