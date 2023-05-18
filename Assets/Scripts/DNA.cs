using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA
{
    public Dictionary<(bool left, bool forward, bool right), float> genes;
    public Stat stat { private set; get; }

    private int dnaLength;

    public DNA()
    {
        genes = new Dictionary<(bool left, bool forward, bool right), float>();
        SetRandom();
    }

    public void SetStat(Stat stat) => this.stat = stat;

    public void SetRandom()
    {
        genes.Clear();
        genes.Add((false, false, false), Random.Range(-90, 91));
        genes.Add((false, false, true), Random.Range(-90, 91));
        genes.Add((false, true, true), Random.Range(-90, 91));
        genes.Add((true, true, true), Random.Range(-90, 91));
        genes.Add((true, false, false), Random.Range(-90, 91));
        genes.Add((true, false, true), Random.Range(-90, 91));
        genes.Add((false, true, false), Random.Range(-90, 91));
        genes.Add((true, true, false), Random.Range(-90, 91));
        dnaLength = genes.Count;
    }

    public void Combine(DNA dna1, DNA dna2)
    {
        int i = 0;      // ù��° DNA, �ι�° DNA ���� ����
        Dictionary<(bool left, bool forward, bool right), float> newGenes = new Dictionary<(bool left, bool forward, bool right), float>();   // ���ο� ������
        foreach(KeyValuePair<(bool left, bool forward, bool right), float> g in genes)
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

    public float GetGene((bool left, bool forward, bool right) seeFeed)
    {
        return genes[seeFeed];
    }
}