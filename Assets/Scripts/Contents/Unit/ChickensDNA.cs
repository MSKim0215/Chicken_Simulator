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

    protected override void SetRandomGenesCode()
    {
        base.SetRandomGenesCode();

        GenesCode.Add(DNAType.Avoidance, UnityEngine.Random.Range(0.1f, 1f));
        GenesCode.Add(DNAType.Cohesion, UnityEngine.Random.Range(0.1f, 1f));
        GenesCode.Add(DNAType.FeedFound, UnityEngine.Random.Range(0.1f, 1f));
        GenesCode.Add(DNAType.SafeFound, UnityEngine.Random.Range(0.1f, 1f));
        dnaCodeLength = GenesCode.Count;
    }

    public override void CombineStat(ChickensDNA dna1, ChickensDNA dna2)
    {
        ChickenStat childStat = new ChickenStat();      // 자식 능력치 코드
        for(int i = 0; i < Enum.GetValues(typeof(StatType)).Length; i++)
        {
            StatType type = (StatType)i;
            if(UnityEngine.Random.Range(0f, 1f) < 0.5f)
            {

            }
        }
    }

    public int Level => StatusCode.Level;
    public int ExpMax => StatusCode.ExpMax;
    public int Hp => StatusCode.Hp;
    public float MoveSpeed => StatusCode.MoveSpeed;
    public int AttackPower => StatusCode.AttackPower;
    public float EatRange => StatusCode.EatRange;
    public int Defense => StatusCode.Defense;
}