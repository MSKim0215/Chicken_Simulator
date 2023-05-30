using System.Collections;
using System.Collections.Generic;
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
                    if (level >= 2) break;
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

    /// <summary>
    /// ���ο� ���� �ο� �Լ�
    /// type: Ÿ��
    /// </summary>
    public void Init(Define.ChickenType type = Define.ChickenType.None)
    {
        this.type = type;

        if (GenesStats.Count > 0) GenesStats.Clear();
        if (NotGenesStats.Count > 0) NotGenesStats.Clear();

        NotGenesStats.Add(NotGenesStatType.Level, Managers.Data.ChickenGroupLevelTable[Define.TableLabel].Level);
        NotGenesStats.Add(NotGenesStatType.ExpMax, Managers.Data.ChickenGroupLevelTable[Define.TableLabel].ExpMax);

        GenesStats.Add(GenesStatType.HpMax, Managers.Data.ChickenGroupStat[Define.TableLabel].HpMax);
        GenesStats.Add(GenesStatType.MoveSpeed, Managers.Data.ChickenGroupStat[Define.TableLabel].MoveSpeed);
        GenesStats.Add(GenesStatType.EatPower, Managers.Data.ChickenGroupStat[Define.TableLabel].EatPower);
        GenesStats.Add(GenesStatType.EatRange, Managers.Data.ChickenGroupStat[Define.TableLabel].EatRange);
        GenesStats.Add(GenesStatType.Defense, Managers.Data.ChickenGroupStat[Define.TableLabel].Defense);

        NowHp = HpMax;
        NowExp = 0;
    }

    public void Init(ChickenStat stat)
    {
        NotGenesStats[NotGenesStatType.Level] = stat.Level;
        NotGenesStats[NotGenesStatType.ExpMax] = stat.ExpMax;

        GenesStats[GenesStatType.HpMax] = stat.HpMax;
        GenesStats[GenesStatType.MoveSpeed] = stat.MoveSpeed;
        GenesStats[GenesStatType.EatPower] = stat.AttackPower;
        GenesStats[GenesStatType.EatRange] = stat.EatRange;
        GenesStats[GenesStatType.Defense] = stat.Defense;
    }

    public Define.ChickenType Type => type;
    public int Level => (int)NotGenesStats[NotGenesStatType.Level];
    public int ExpMax => (int)NotGenesStats[NotGenesStatType.ExpMax];
    public int HpMax => (int)GenesStats[GenesStatType.HpMax];
    public float MoveSpeed => (float)GenesStats[GenesStatType.MoveSpeed];
    public int AttackPower => (int)GenesStats[GenesStatType.EatPower];
    public float EatRange => (float)GenesStats[GenesStatType.EatRange];
    public int Defense => (int)GenesStats[GenesStatType.Defense];
}