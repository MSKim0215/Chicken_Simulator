using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public enum NotGenesStatType
{
    Level,
    ExpMax
}

public class ChickenStat : BaseStat
{
    private Define.ChickenType type;
    private int nowExp;     // 현재 경험치량

    public Data.ChickenStat nextStat;

    public Dictionary<NotGenesStatType, object> NotGenesStats { private set; get; } = new Dictionary<NotGenesStatType, object>();   // 유전 불가능한 스탯

    public int NowExp 
    { 
        get => nowExp; 
        set
        {
            if (value == 0) return;     // 획득이 없으면 종료
            nowExp = value;

            // TODO: 레벨업 체크 로직
            int level = Level;

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
            }

            if(level != Level)
            {
                Init(type, level);
            }
        }
    }

    public void Init(Define.ChickenType type = Define.ChickenType.None, int level = 1)
    {
        this.type = type;

        if (GenesStats.Count > 0) GenesStats.Clear();
        if (NotGenesStats.Count > 0) NotGenesStats.Clear();

        switch (this.type)
        {
            case Define.ChickenType.Egg: SetEggStat(level); break;
            case Define.ChickenType.Chick: SetChickStat(level); break;
            case Define.ChickenType.Chicken: SetChickenStat(level); break;
        }

        NowHp = HpMax;
        NowExp = 0;
    }

    private void SetEggStat(int level)
    {
        NotGenesStats.Add(NotGenesStatType.Level, Managers.Data.EggStatDict[level].Level);
        NotGenesStats.Add(NotGenesStatType.ExpMax, Managers.Data.EggStatDict[level].ExpMax);

        GenesStats.Add(GenesStatType.HpMax, Managers.Data.EggStatDict[level].HpMax);
        GenesStats.Add(GenesStatType.MoveSpeed, Managers.Data.EggStatDict[level].MoveSpeed);
        GenesStats.Add(GenesStatType.EatPower, Managers.Data.EggStatDict[level].AttackPower);
        GenesStats.Add(GenesStatType.EatRange, Managers.Data.EggStatDict[level].EatRange);
        GenesStats.Add(GenesStatType.Defense, Managers.Data.EggStatDict[level].Defense);
    }

    private void SetChickStat(int level)
    {
        NotGenesStats.Add(NotGenesStatType.Level, Managers.Data.ChickStatDict[level].Level);
        NotGenesStats.Add(NotGenesStatType.ExpMax, Managers.Data.ChickStatDict[level].ExpMax);

        GenesStats.Add(GenesStatType.HpMax, Managers.Data.ChickStatDict[level].HpMax);
        GenesStats.Add(GenesStatType.MoveSpeed, Managers.Data.ChickStatDict[level].MoveSpeed);
        GenesStats.Add(GenesStatType.EatPower, Managers.Data.ChickStatDict[level].AttackPower);
        GenesStats.Add(GenesStatType.EatRange, Managers.Data.ChickStatDict[level].EatRange);
        GenesStats.Add(GenesStatType.Defense, Managers.Data.ChickStatDict[level].Defense);
    }

    private void SetChickenStat(int level)
    {
        NotGenesStats.Add(NotGenesStatType.Level, Managers.Data.ChickenStatDict[level].Level);
        NotGenesStats.Add(NotGenesStatType.ExpMax, Managers.Data.ChickenStatDict[level].ExpMax);

        GenesStats.Add(GenesStatType.HpMax, Managers.Data.ChickenStatDict[level].HpMax);
        GenesStats.Add(GenesStatType.MoveSpeed, Managers.Data.ChickenStatDict[level].MoveSpeed);
        GenesStats.Add(GenesStatType.EatPower, Managers.Data.ChickenStatDict[level].AttackPower);
        GenesStats.Add(GenesStatType.EatRange, Managers.Data.ChickenStatDict[level].EatRange);
        GenesStats.Add(GenesStatType.Defense, Managers.Data.ChickenStatDict[level].Defense);
    }

    public int Level => (int)NotGenesStats[NotGenesStatType.Level];
    public int ExpMax => (int)NotGenesStats[NotGenesStatType.ExpMax];
    public int HpMax => (int)GenesStats[GenesStatType.HpMax];
    public float MoveSpeed => (float)GenesStats[GenesStatType.MoveSpeed];
    public int AttackPower => (int)GenesStats[GenesStatType.EatPower];
    public float EatRange => (float)GenesStats[GenesStatType.EatRange];
    public int Defense => (int)GenesStats[GenesStatType.Defense];
}