using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Hpbar : UI_Base
{
    private enum GameObjects
    {
        Slider_Hpbar
    }

    private BaseStat stat;

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        stat = transform.parent.GetComponent<BaseStat>();
    }

    private void Update()
    {
        Transform parent = transform.parent;
        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y);
        transform.rotation = Camera.main.transform.rotation;

        float ratio = (float)stat.NowHp / (int)stat.GenesStats[GenesStatType.HpMax];
        SetHpRatio(ratio);
    }

    public void SetHpRatio(float _ratio)
    {
        GetObject((int)GameObjects.Slider_Hpbar).GetComponent<Slider>().value = _ratio;
    }
}