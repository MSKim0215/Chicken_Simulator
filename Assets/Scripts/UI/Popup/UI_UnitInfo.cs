using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitInfo : UI_Popup
{
    private enum Texts
    {
        Text_Name,
        Text_Level,
        Text_Hp,
        Text_Exp
    }

    private enum GameObjects
    {
        Img_Mutant
    }

    private enum Sliders
    {
        Slider_Hpbar,
        Slider_Expbar
    }

    private ChickensBrain target;

    public void SetTarget(ChickensBrain target) => this.target = target;

    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Slider>(typeof(Sliders));

        RefreshUI();
    }

    private void Update()
    {
        if (target == null) return;

        //float hpRatio = (int)target.DNA.StatusCode.GenesStats[StatType.Hp] / (float)target.DNA.StatusCode.HpMax;
        //SetHpRatio(hpRatio);

        //Data.ChickenStat next = target.dna.stat.GetComponent<Data.ChickenStat>();
        //float expRatio = target.feedsFound / next.ExpMax;
        //SetExpRatio(expRatio);
    }

    private void RefreshUI()
    {
        GetText((int)Texts.Text_Name).text = target.DNA.StatusCode.gameObject.tag;
        //GetText((int)Texts.Text_Level).text = $"Lv.{target.DNA.StatusCode.GenesStats[StatType.Level]}";
        GetObject((int)GameObjects.Img_Mutant).SetActive(target.DNA.isMutant);
    }

    private void SetHpRatio(float ratio)
    {
        GetSlider((int)Sliders.Slider_Hpbar).value = ratio;
        //GetText((int)Texts.Text_Hp).text = $"{target.DNA.StatusCode.GenesStats[StatType.Hp]}/{target.DNA.StatusCode.HpMax}";
    }

    public void SetExpRatio(float ratio)
    {
        GetSlider((int)Sliders.Slider_Expbar).value = ratio;
        //Data.ChickenStat next = target.DNA.StatusCode.GetComponent<Data.ChickenStat>();
        //GetText((int)Texts.Text_Exp).text = $"{target.feedsFound}/{next.ExpMax}";
    }
}