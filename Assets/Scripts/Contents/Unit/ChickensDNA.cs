using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickensDNA : RootDNA
{
    public ChickenStat StatusCode { private set; get; }     // 능력치 코드

    public ChickensDNA()
    {
        WorldObjectType = Define.WorldObject.Chicken;
        SetRandomGenesCode();
    }

    protected override void SetRandomGenesCode()
    {
        base.SetRandomGenesCode();

        GenesCode.Add(DNAType.Avoidance, 1f);
        GenesCode.Add(DNAType.Cohesion, 1f);
        GenesCode.Add(DNAType.FeedFound, 1f);
        GenesCode.Add(DNAType.SafeFound, 1f);
        dnaCodeLength = GenesCode.Count;
    }

    public void SetStatus(ChickenStat stat, Define.ChickenType type)
    {
        StatusCode = stat;
        StatusCode.Init(type);
    }
}