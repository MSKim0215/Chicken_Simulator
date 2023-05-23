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

public class DNA
{
    public Dictionary<DNAType, float> genes;        // 유전자 코드
    public Stat stat { private set; get; }

    private int dnaLength;

    public DNA()
    {
        genes = new Dictionary<DNAType, float>();
        SetRandom();
    }

    public void SetStat(Stat stat, Define.ChickenType type)
    {
        this.stat = stat;
        stat.Init(type);
    }

    public void SetRandom()
    {
        genes.Clear();
        genes.Add(DNAType.Avoidance, 1f);
        genes.Add(DNAType.Cohesion, 1f);
        genes.Add(DNAType.FeedFound, 1f);
        genes.Add(DNAType.SafeFound, 1f);
        dnaLength = genes.Count;
    }

    public void CombineGenes(DNA dna1, DNA dna2)
    {
        int i = 0;      // 첫번째 DNA, 두번째 DNA 교차 조합
        Dictionary<DNAType, float> newGenes = new Dictionary<DNAType, float>();   // 새로운 유전자
        foreach(KeyValuePair<DNAType, float> g in genes)
        {
            if(i < dnaLength / 2)
            {   // dna 길이의 절반은 첫번째 DNA 계승
                newGenes.Add(g.Key, dna1.genes[g.Key]);
            }
            else
            {   // 이후 모든 절반은 두번째 DNA 계승
                newGenes.Add(g.Key, dna2.genes[g.Key]);
            }
            i++;
        }
        genes = newGenes;
    }

    public void CombineStat(DNA dna1, DNA dna2)
    {
        Stat newStat = new Stat();      // 새로운 스탯
        for(int i = 0; i < Enum.GetValues(typeof(StatType)).Length; i++)
        {
            StatType statType = (StatType)i;

            if(UnityEngine.Random.Range(0f, 1f) < 0.5f)
            {   // 50% 확률로 첫번째 스탯 중 하나 계승 (중복 스탯x)
                if(!newStat.Stats.ContainsKey(statType))
                {
                    newStat.Stats.Add(statType, dna1.stat.Stats[statType]);
                }
            }
            else
            {   // 50% 확률로 두번째 스탯 중 하나 계승 (중복 스탯x)
                if (!newStat.Stats.ContainsKey(statType))
                {
                    newStat.Stats.Add(statType, dna2.stat.Stats[statType]);
                }
            }
        }
        stat = newStat;
    }

    public float GetGene(DNAType seeFeed)
    {
        return genes[seeFeed];
    }
}