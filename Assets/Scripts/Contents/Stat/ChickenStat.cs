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
    private int nowExp;     // 현재 경험치량

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
            int expMax = ExpMax;

            while(true)
            {
                if(type == Define.ChickenType.Chick)
                {   // TODO: 병아리는 최대 레벨 5
                    if (level >= 2) break;
                    if (nowExp < expMax) break;
                }
                else if(type == Define.ChickenType.Chicken)
                {   // TODO: 닭은 최대 레벨이 없음
                    if (nowExp < expMax) break;
                }
                level++;
                expMax = (level * expMax);

                // TODO: 레벨업 하면 스탯 중 하나 상승
                int randStat = Random.Range(0, (int)GenesStatType.EndCount);        // 유전 가능 스탯 중 하나 상승
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
    /// 새로운 스탯 부여 함수
    /// type: 타입
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