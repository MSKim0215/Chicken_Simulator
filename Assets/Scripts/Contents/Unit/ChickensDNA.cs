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

        GenesCode.Add(DNAType.Avoidance, Random.Range(0.1f, 1f));
        GenesCode.Add(DNAType.Cohesion, Random.Range(0.1f, 1f));
        GenesCode.Add(DNAType.FeedFound, Random.Range(0.1f, 1f));
        GenesCode.Add(DNAType.SafeFound, Random.Range(0.1f, 1f));
        dnaCodeLength = GenesCode.Count;
    }

    public int Level => StatusCode.Level;
    public int ExpMax => StatusCode.ExpMax;
    public int Hp => StatusCode.Hp;
    public float MoveSpeed => StatusCode.MoveSpeed;
    public int AttackPower => StatusCode.AttackPower;
    public float EatRange => StatusCode.EatRange;
    public int Defense => StatusCode.Defense;
}