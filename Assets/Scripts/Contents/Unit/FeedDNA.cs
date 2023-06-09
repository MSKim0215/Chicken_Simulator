using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedDNA : RootDNA
{
    public FeedStat StatusCode { private set; get; }        // 능력치 코드

    public FeedDNA()
    {
        WorldObjectType = Define.WorldObject.NeutralGroup;
        SetRandomGenesCode();
    }

    protected override void SetRandomGenesCode()
    {
        base.SetRandomGenesCode();

        GenesCode.Add(DNAType.Avoidance, 0f);
        GenesCode.Add(DNAType.Cohesion, 0f);
        GenesCode.Add(DNAType.FeedFound, 0f);
        GenesCode.Add(DNAType.SafeFound, 0f);
        dnaCodeLength = GenesCode.Count;
    }

    public void SetStatus(FeedStat stat)
    {
        StatusCode = stat;
        StatusCode.Init();
    }
}