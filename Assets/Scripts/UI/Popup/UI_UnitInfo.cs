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

    private Brain target;

    public void SetTarget(Brain target) => this.target = target;

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

        float hpRatio = (int)target.dna.stat.Stats[StatType.Hp] / (float)target.dna.stat.HpMax;
        SetHpRatio(hpRatio);

        float expRatio = target.feedsFound / (int)target.dna.stat.Stats[StatType.ExpMax];
        SetExpRatio(expRatio);
    }

    private void RefreshUI()
    {
        GetText((int)Texts.Text_Name).text = target.dna.stat.gameObject.tag;
        GetText((int)Texts.Text_Level).text = $"Lv.{target.dna.stat.Stats[StatType.Level]}";
        GetObject((int)GameObjects.Img_Mutant).SetActive(target.dna.isMutant);
    }

    private void SetHpRatio(float ratio)
    {
        GetSlider((int)Sliders.Slider_Hpbar).value = ratio;
        GetText((int)Texts.Text_Hp).text = $"{target.dna.stat.Stats[StatType.Hp]}/{target.dna.stat.HpMax}";
    }

    private void SetExpRatio(float ratio)
    {
        GetSlider((int)Sliders.Slider_Expbar).value = ratio;
        GetText((int)Texts.Text_Exp).text = $"{target.feedsFound}/{target.dna.stat.Stats[StatType.ExpMax]}";
    }
}