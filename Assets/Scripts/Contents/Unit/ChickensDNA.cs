using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickensDNA : RootDNA
{
    public ChickenStat StatusCode { private set; get; }     // �ɷ�ġ �ڵ�

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
        Dictionary<DNAType, float> childGenes = new Dictionary<DNAType, float>();       // �ڽ� ������ �ڵ�
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
            {   // dna ������ ������ �ƹ��� DNA ���
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
            {   // dna ������ ������ ��Ӵ� DNA ���
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