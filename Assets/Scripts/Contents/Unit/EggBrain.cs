using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBrain : BaseBrain
{
    private Define.ChickenType type;                    // 종류

    public ChickensDNA DNA { private set; get; }        // 유전자 DNA
    public int HatchCounting { private set; get; }      // 현재 부화일
    public int HatchCountMax { private set; get; }      // 부화 시점

    public override void Init()
    {
        type = Define.ChickenType.Egg;

        DNA = new ChickensDNA();
        DNA.Init(GetComponent<ChickenStat>(), type);

        HatchCounting = 0;
        HatchCountMax = 2;
    }

    public bool CheckHatchAble()
    {
        if (HatchCounting >= HatchCountMax) return true;
        return false;
    }

    public void AddHatchCount() => HatchCounting++;
}