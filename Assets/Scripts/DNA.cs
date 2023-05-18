using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DNAType
{
    Avoidance,      // ȸ�� ����ġ
    Cohesion,       // ���� ����ġ
    FeedFound,      // ���� Ž�� ����ġ
    SafeFound       // ���� ���� Ž�� ����ġ
}

public class DNA
{
    public Dictionary<DNAType, float> genes;        // ������ �ڵ�
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
        int i = 0;      // ù��° DNA, �ι�° DNA ���� ����
        Dictionary<DNAType, float> newGenes = new Dictionary<DNAType, float>();   // ���ο� ������
        foreach(KeyValuePair<DNAType, float> g in genes)
        {
            if(i < dnaLength / 2f)
            {   // dna ������ ������ ù��° DNA ���
                newGenes.Add(g.Key, dna1.genes[g.Key]);
            }
            else
            {   // ���� ��� ������ �ι�° DNA ���
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