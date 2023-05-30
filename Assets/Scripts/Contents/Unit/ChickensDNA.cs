using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickensDNA : RootDNA
{
    public ChickenStat StatusCode { private set; get; }     // 능력치 코드

    public void Init(ChickenStat stat, Define.ChickenType type)
    {
        WorldObjectType = Define.WorldObject.ChickGroup;

        StatusCode = stat;
        StatusCode.Init(type);

        SetRandomGenesCode();
    }

    public void Init(ChickenStat stat)
    {
        StatusCode.Init(stat);
    }

    protected override void SetRandomGenesCode()
    {
        base.SetRandomGenesCode();

        GenesCode.Add(DNAType.Avoidance, UnityEngine.Random.Range(0.1f, 1f));
        GenesCode.Add(DNAType.Cohesion, UnityEngine.Random.Range(0.1f, 1f));
        GenesCode.Add(DNAType.FeedFound, UnityEngine.Random.Range(0.1f, 1f));
        GenesCode.Add(DNAType.SafeFound, UnityEngine.Random.Range(0.1f, 1f));
        dnaCodeLength = GenesCode.Count;
    }

    public override void CombineGenes(ChickensDNA fotherDNA, ChickensDNA motherDNA)
    {
        int index = 0;
        Dictionary<DNAType, float> childGenes = new Dictionary<DNAType, float>();       // 자식 유전자 코드
        foreach(KeyValuePair<DNAType, float> type in GenesCode)
        {
            if (index < dnaCodeLength / 2)
            {
                childGenes.Add(type.Key, fotherDNA.GenesCode[type.Key]);
            }
            else
            {
                childGenes.Add(type.Key, motherDNA.GenesCode[type.Key]);
            }
            index++;
        }
        GenesCode = childGenes;
    }

    public override void CombineStat(ChickensDNA fotherDNA, ChickensDNA motherDNA, float statValue = 1f)
    {
        int statLength = Enum.GetValues(typeof(GenesStatType)).Length - 1;
        for (int i = 0; i < statLength; i++)
        {
            GenesStatType type = (GenesStatType)i;
            if(i < statLength / 2)
            {   // dna 길이의 절반은 아버지 DNA 계승
                object tempStat = fotherDNA.StatusCode.GenesStats[type];
                if(tempStat is int)
                {
                    StatusCode.GenesStats[type] = (int)((int)tempStat * statValue);
                }
                else if(tempStat is float)
                {
                    StatusCode.GenesStats[type] = (float)tempStat * statValue;
                }
            }
            else
            {   // dna 길이의 절반은 어머니 DNA 계승
                object tempStat = motherDNA.StatusCode.GenesStats[type];
                if(tempStat is int)
                {
                    StatusCode.GenesStats[type] = (int)((int)tempStat * statValue);
                }
                else if(tempStat is float)
                {
                    StatusCode.GenesStats[type] = (float)tempStat * statValue;
                }
            }
        }
    }

    public Define.ChickenType Type => StatusCode.Type;
    public int Level => StatusCode.Level;
    public int ExpMax => StatusCode.ExpMax;
    public int Hp => StatusCode.HpMax;
    public float MoveSpeed => StatusCode.MoveSpeed;
    public int AttackPower => StatusCode.AttackPower;
    public float EatRange => StatusCode.EatRange;
    public int Defense => StatusCode.Defense;
}