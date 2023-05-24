using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenStat : Stat
{
    private Define.ChickenType type;
    private int nowExp;
    
    public int NowExp 
    { 
        get => nowExp; 
        set
        {
            nowExp = value;

            // TODO: 레벨업 체크 로직
            int level = (int)Stats[StatType.Level];
            Data.ChickenStat stat;

            while(true)
            {
                if (!Managers.Data.ChickStatDict.TryGetValue(level + 1, out stat)) break;
                if (nowExp < stat.ExpMax) break;
                level++;
            }

            if(level != (int)Stats[StatType.Level])
            {
                Init(type, level);
            }
        }
    }

    public override void Init(Define.ChickenType type = Define.ChickenType.None, int level = 1)
    {
        this.type = type;

        if (Stats.Count > 0) Stats.Clear();

        Stats.Add(StatType.Level, Managers.Data.ChickStatDict[level].Level);
        Stats.Add(StatType.ExpMax, Managers.Data.ChickStatDict[level].ExpMax);

        Stats.Add(StatType.Hp, Managers.Data.ChickStatDict[level].HpMax);
        Stats.Add(StatType.MoveSpeed, Managers.Data.ChickStatDict[level].MoveSpeed);
        Stats.Add(StatType.AttackPower, Managers.Data.ChickStatDict[level].AttackPower);
        Stats.Add(StatType.EatRange, Managers.Data.ChickStatDict[level].EatRange);
        Stats.Add(StatType.Defense, Managers.Data.ChickStatDict[level].Defense);

        HpMax = (int)Stats[StatType.Hp];
        NowExp = 0;
    }
}