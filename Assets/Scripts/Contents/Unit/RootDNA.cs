using System;
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

public class RootDNA : MonoBehaviour
{
    protected int dnaCodeLength;        // ������ �ڵ� ����

    public bool isMutant = false;     // �������� ����

    public Dictionary<DNAType, float> GenesCode { get; protected set; } = new Dictionary<DNAType, float>();     // ������ �ڵ�
    public Stat StatusCode { get; protected set; }      // �ɷ�ġ �ڵ�
    public Define.WorldObject WorldObjectType { get; protected set; } = Define.WorldObject.Unknown;

    public RootDNA()
    {
        SetRandomGenesCode();
    }

    protected virtual void SetRandomGenesCode()
    {
        GenesCode.Clear();
        //GenesCode.Add(DNAType.Avoidance, 1f);
        //GenesCode.Add(DNAType.Cohesion, 1f);
        //GenesCode.Add(DNAType.FeedFound, 1f);
        //GenesCode.Add(DNAType.SafeFound, 1f);
        //dnaCodeLength = GenesCode.Count;
    }

    public void CombineGenes(DNA dna1, DNA dna2)
    {
        int i = 0;      // ù��° DNA, �ι�° DNA ���� ����
        Dictionary<DNAType, float> newGenes = new Dictionary<DNAType, float>();   // ���ο� ������
        foreach (KeyValuePair<DNAType, float> g in GenesCode)
        {
            if (i < dnaCodeLength / 2)
            {   // dna ������ ������ ù��° DNA ���
                newGenes.Add(g.Key, dna1.genes[g.Key]);
            }
            else
            {   // ���� ��� ������ �ι�° DNA ���
                newGenes.Add(g.Key, dna2.genes[g.Key]);
            }
            i++;
        }
        GenesCode = newGenes;
    }

    public void CombineStat(DNA dna1, DNA dna2)
    {
        Stat newStat = new Stat();      // ���ο� ����
        for (int i = 0; i < Enum.GetValues(typeof(StatType)).Length; i++)
        {
            StatType statType = (StatType)i;

            if (UnityEngine.Random.Range(0f, 1f) < 0.5f)
            {   // 50% Ȯ���� ù��° ���� �� �ϳ� ��� (�ߺ� ����x)
                if (!newStat.Stats.ContainsKey(statType))
                {
                    newStat.Stats.Add(statType, dna1.stat.Stats[statType]);
                }
            }
            else
            {   // 50% Ȯ���� �ι�° ���� �� �ϳ� ��� (�ߺ� ����x)
                if (!newStat.Stats.ContainsKey(statType))
                {
                    newStat.Stats.Add(statType, dna2.stat.Stats[statType]);
                }
            }
        }
        StatusCode = newStat;
    }
}