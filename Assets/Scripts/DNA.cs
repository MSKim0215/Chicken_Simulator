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
        int i = 0;      // ù��° DNA, �ι�° DNA ���� ����
        Dictionary<bool, float> newGenes = new Dictionary<bool, float>();   // ���ο� ������
        foreach(KeyValuePair<bool, float> g in genes)
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

    public float GetGene(bool front)
    {
        return genes[front];
    }
}