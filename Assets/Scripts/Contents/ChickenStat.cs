using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenStat : Stat
{
    private int nowExp;
    
    public int NowExp 
    { 
        get => nowExp; 
        set
        {
            nowExp = value;

            // TODO: 레벨업 체크 로직
            int level = (int)Stats[StatType.Level];
            Data.Stat stat;

            while(true)
            {
                if (!Managers.Data.StatDict.TryGetValue(level + 1, out stat)) break;
                if (nowExp < stat.totalExp) break;
                level++;
            }

            if(level != (int)Stats[StatType.Level])
            {
                Debug.Log("레벨 상승");
                Stats[StatType.Level] = level;
                SetStat((int)Stats[StatType.Level]);
            }
        }
    }

    private void Start()
    {
        Stats.Add(StatType.Hp, 5);
        Stats.Add(StatType.MoveSpeed, 1f);
        Stats.Add(StatType.AttackPower, 1);
        Stats.Add(StatType.EatRange, 0.13f);
        Stats.Add(StatType.Defense, 0);

        HpMax = (int)Stats[StatType.Hp];

        Stats.Add(StatType.Level, 1);
        NowExp = 0;
        Stats.Add(StatType.ExpMax, 2);
    }

    public void SetStat(int level)
    {
        Dictionary<int, Data.Stat> datas = new Dictionary<int, Data.Stat>();
        Data.Stat stat = datas[level];
    }
}