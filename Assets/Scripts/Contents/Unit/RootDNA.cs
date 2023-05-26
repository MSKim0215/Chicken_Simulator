using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DNAType
{
    Avoidance,      // 회피 가중치
    Cohesion,       // 밀집 가중치
    FeedFound,      // 먹이 탐색 가중치
    SafeFound       // 안전 구역 탐색 가중치
}

public class RootDNA 
{
    protected int dnaCodeLength;        // 유전자 코드 길이

    public bool isMutant = false;     // 돌연변이 여부

    public Dictionary<DNAType, float> GenesCode { get; protected set; } = new Dictionary<DNAType, float>();     // 유전자 코드
    public Define.WorldObject WorldObjectType { get; protected set; } = Define.WorldObject.Unknown;

    public RootDNA()
    {
        SetRandomGenesCode();
    }

    protected virtual void SetRandomGenesCode()
    {
        GenesCode.Clear();
    }

    public void CombineGenes(ChickensDNA dna1, ChickensDNA dna2)
    {
        int i = 0;      // 첫번째 DNA, 두번째 DNA 교차 조합
        Dictionary<DNAType, float> newGenes = new Dictionary<DNAType, float>();   // 새로운 유전자
        foreach (KeyValuePair<DNAType, float> g in GenesCode)
        {
            if (i < dnaCodeLength / 2)
            {   // dna 길이의 절반은 첫번째 DNA 계승
                newGenes.Add(g.Key, dna1.GenesCode[g.Key]);
            }
            else
            {   // 이후 모든 절반은 두번째 DNA 계승
                newGenes.Add(g.Key, dna2.GenesCode[g.Key]);
            }
            i++;
        }
        GenesCode = newGenes;
    }

    public virtual void CombineStat(ChickensDNA dna1, ChickensDNA dna2)
    {
        Stat newStat = new Stat();      // 새로운 스탯
        for (int i = 0; i < Enum.GetValues(typeof(StatType)).Length; i++)
        {
            StatType statType = (StatType)i;

            if (UnityEngine.Random.Range(0f, 1f) < 0.5f)
            {   // 50% 확률로 첫번째 스탯 중 하나 계승 (중복 스탯x)
                if (!newStat.Stats.ContainsKey(statType))
                {
                    newStat.Stats.Add(statType, dna1.StatusCode.Stats[statType]);
                }
            }
            else
            {   // 50% 확률로 두번째 스탯 중 하나 계승 (중복 스탯x)
                if (!newStat.Stats.ContainsKey(statType))
                {
                    newStat.Stats.Add(statType, dna2.StatusCode.Stats[statType]);
                }
            }
        }
        //StatusCode = newStat;
    }
}