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

    public void SetStat(Stat stat) => this.stat = stat;

    public void SetRandom()
    {
        genes.Clear();
        genes.Add(DNAType.Avoidance, 1f);
        genes.Add(DNAType.Cohesion, 1f);
        genes.Add(DNAType.FeedFound, 1f);
        genes.Add(DNAType.SafeFound, 1f);
        dnaLength = genes.Count;
    }

    public void Combine(DNA dna1, DNA dna2)
    {
        int i = 0;      // 첫번째 DNA, 두번째 DNA 교차 조합
        Dictionary<DNAType, float> newGenes = new Dictionary<DNAType, float>();   // 새로운 유전자
        foreach(KeyValuePair<DNAType, float> g in genes)
        {
            if(i < dnaLength / 2f)
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

    public float GetGene(DNAType seeFeed)
    {
        return genes[seeFeed];
    }
}