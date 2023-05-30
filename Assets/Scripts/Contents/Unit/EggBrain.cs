using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBrain : BaseBrain
{
    public ChickensDNA DNA { private set; get; }        // ������ DNA
    public int HatchCounting { private set; get; }      // ���� ��ȭ��
    public int HatchCountMax { private set; get; }      // ��ȭ ����

    public override void Init()
    {
        HatchCounting = 0;
        HatchCountMax = 2;
    }

    /// <summary>
    /// �ű� DNA ����
    /// </summary>
    public void MakeDNA()
    {
        DNA = new ChickensDNA();
        DNA.Init(GetComponent<ChickenStat>(), Define.ChickenType.Egg);
    }

    /// <summary>
    /// ���� DNA ����
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