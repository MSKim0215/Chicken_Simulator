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
    private int nowExp;     // ���� ����ġ��

    public Dictionary<NotGenesStatType, object> NotGenesStats { private set; get; } = new Dictionary<NotGenesStatType, object>();   // ���� �Ұ����� ����

    public int NowExp 
    { 
        get => nowExp; 
        set
        {
            if (value == 0) return;     // ȹ���� ������ ����
            nowExp = value;

            // TODO: ������ üũ ����
            int level = Level;
            int expMax = ExpMax;

            while(true)
            {
                if(type == Define.ChickenType.Chick)
                {   // TODO: ���Ƹ��� �ִ� ���� 5
                    if (level >= 5) break;
                    if (nowExp < expMax) break;
                }
                else if(type == Define.ChickenType.Chicken)
                {   // TODO: ���� �ִ� ������ ����
                    if (nowExp < expMax) break;
                }
                level++;
                expMax = (level * expMax);

                // TODO: ������ �ϸ� ���� �� �ϳ� ���
                int randStat = Random.Range(0, (int)GenesStatType.EndCount);        // ���� ���� ���� �� �ϳ� ���
                GenesStatType selectStatType = (GenesStatType)randStat;
                if(GenesStats.ContainsKey(selectStatType))
                {
                    object statValue = GenesStats[selectStatType];
                    if(statValue is int)
                    {
                        int intValue = (int)statValue;
                        intValue += 1;
                        GenesStats[selectStatType] = intValue;
                    }
                    else if(statValue is float)
                    {
                        float floatValue = (float)statValue;
                        floatValue += 0.1f;
                        GenesStats[selectStatType] = floatValue;
                    }
                }
            }

            NotGenesStats[NotGenesStatType.Level] = level;
            NotGenesStats[NotGenesStatType.ExpMax] = expMax;
 
            //if(level != Level)
            //{
            //    Init(type, level);
            //}
        }
    }

    public void Init(Define.ChickenType type = Define.ChickenType.None, int level = 1)
    {
        this.type = type;

        if (GenesStats.Count > 0) GenesStats.Clear();
        if (NotGenesStats.Count > 0) NotGenesStats.Clear();

        NotGenesStats.Add(NotGenesStatType.Level, Managers.Data.ChickenGroupLevelTable[level].Level);
        NotGenesStats.Add(NotGenesStatType.ExpMax, Managers.Data.ChickenGroupLevelTable[level].ExpMax);

        GenesStats.Add(GenesStatType.HpMax, Managers.Data.ChickenGroupStat[level].HpMax);
        GenesStats.Add(GenesStatType.MoveSpeed, Managers.Data.ChickenGroupStat[level].MoveSpeed);
        GenesStats.Add(GenesStatType.EatPower, Managers.Data.ChickenGroupStat[level].EatPower);
        GenesStats.Add(GenesStatType.EatRange, Managers.Data.ChickenGroupStat[level].EatRange);
        GenesStats.Add(GenesStatType.Defense, Managers.Data.ChickenGroupStat[level].Defense);

        NowHp = HpMax;
        NowExp = 0;
    }

    public int Level => (int)NotGenesStats[NotGenesStatType.Level];
    public int ExpMax => (int)NotGenesStats[NotGenesStatType.ExpMax];
    public int HpMax => (int)GenesStats[GenesStatType.HpMax];
    public float MoveSpeed => (float)GenesStats[GenesStatType.MoveSpeed];
    public int AttackPower => (int)GenesStats[GenesStatType.EatPower];
    public float EatRange => (float)GenesStats[GenesStatType.EatRange];
    public int Defense => (int)GenesStats[GenesStatType.Defense];
}