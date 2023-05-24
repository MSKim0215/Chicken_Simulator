using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenStat : Stat
{
    private Define.ChickenType type;
    private int nowExp;

    public Data.ChickenStat nextStat;

    public int NowExp 
    { 
        get => nowExp; 
        set
        {
            nowExp = value;

            // TODO: 레벨업 체크 로직
            int level = (int)Stats[StatType.Level];

            while(true)
            {
                if(type == Define.ChickenType.Chick)
                {
                    if (!Managers.Data.ChickStatDict.TryGetValue(level + 1, out nextStat)) break;
                    if (nowExp < nextStat.ExpMax) break;
                }
                else if(type == Define.ChickenType.Chicken)
                {
                    if(!Managers.Data.ChickenStatDict.TryGetValue(level + 1, out nextStat)) break;
                    if (nowExp < nextStat.ExpMax) break;
                }
                level++;
                Debug.Log("레벨 상승! 현재: " + level);
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

        switch(this.type)
        {
            case Define.ChickenType.Chick: SetChickStat(level); break;
            case Define.ChickenType.Chicken: SetChickenStat(level); break;
        }

        HpMax = (int)Stats[StatType.Hp];
        NowExp = 0;
    }

    private void SetChickStat(int level)
    {
        Stats.Add(StatType.Level, Managers.Data.ChickStatDict[level].Level);
        Stats.Add(StatType.ExpMax, Managers.Data.ChickStatDict[level].ExpMax);

        Stats.Add(StatType.Hp, Managers.Data.ChickStatDict[level].HpMax);
        Stats.Add(StatType.MoveSpeed, Managers.Data.ChickStatDict[level].MoveSpeed);
        Stats.Add(StatType.AttackPower, Managers.Data.ChickStatDict[level].AttackPower);
        Stats.Add(StatType.EatRange, Managers.Data.ChickStatDict[level].EatRange);
        Stats.Add(StatType.Defense, Managers.Data.ChickStatDict[level].Defense);
    }

    private void SetChickenStat(int level)
    {
        Stats.Add(StatType.Level, Managers.Data.ChickenStatDict[level].Level);
        Stats.Add(StatType.ExpMax, Managers.Data.ChickenStatDict[level].ExpMax);

        Stats.Add(StatType.Hp, Managers.Data.ChickenStatDict[level].HpMax);
        Stats.Add(StatType.MoveSpeed, Managers.Data.ChickenStatDict[level].MoveSpeed);
        Stats.Add(StatType.AttackPower, Managers.Data.ChickenStatDict[level].AttackPower);
        Stats.Add(StatType.EatRange, Managers.Data.ChickenStatDict[level].EatRange);
        Stats.Add(StatType.Defense, Managers.Data.ChickenStatDict[level].Defense);
    }
}