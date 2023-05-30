using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBrain : BaseBrain
{
    public ChickensDNA DNA { private set; get; }        // 유전자 DNA
    public int HatchCounting { private set; get; }      // 현재 부화일
    public int HatchCountMax { private set; get; }      // 부화 시점

    public override void Init()
    {
        HatchCounting = 0;
        HatchCountMax = 2;
    }

    /// <summary>
    /// 신규 DNA 생성
    /// </summary>
    public void MakeDNA()
    {
        DNA = new ChickensDNA();
        DNA.Init(GetComponent<ChickenStat>(), Define.ChickenType.Egg);
    }

    /// <summary>
    /// 기존 DNA 복사
    /// </summary>
    public void CopyDNA(ChickensDNA targetDNA)
    {
        if (DNA == null) return;

        DNA.Init(targetDNA.StatusCode);
    }

    public bool CheckHatchAble()
    {
        if (HatchCounting >= HatchCountMax) return true;
        return false;
    }

    public void AddHatchCount() => HatchCounting++;
}