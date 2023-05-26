using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBrain : BaseBrain
{
    private Define.ChickenType type;                    // ����

    public ChickensDNA DNA { private set; get; }        // ������ DNA
    public int HatchCounting { private set; get; }      // ���� ��ȭ��
    public int HatchCountMax { private set; get; }      // ��ȭ ����

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