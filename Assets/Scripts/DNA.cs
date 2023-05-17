using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA
{
    public Dictionary<bool, float> genes;

    private int dnaLength;

    public DNA()
    {
        genes = new Dictionary<bool, float>();
        SetRandom();
    }

    public void SetRandom()
    {
        genes.Clear();
        genes.Add(false, Random.Range(-90, 91));
        genes.Add(true, Random.Range(-90, 91));
        dnaLength = genes.Count;
    }

    public void Combine(DNA dna1, DNA dna2)
    {
        int i = 0;      // 첫번째 DNA, 두번째 DNA 교차 조합
        Dictionary<bool, float> newGenes = new Dictionary<bool, float>();   // 새로운 유전자
        foreach(KeyValuePair<bool, float> g in genes)
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

    public float GetGene(bool front)
    {
        return genes[front];
    }
}