using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ExpBar : UI_Scene
{
    private enum Images
    {
        Img_Exp
    }

    private enum Texts
    {
        Text_Exp
    }

    public override void Init()
    {
        if (isInit) return;
        isInit = true;

        base.Init();

        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));
    }

    public void SetExp(int _nowValue = 0, int _maxValue = 0)
    {
        if (!isInit) Init();

        float point = (float)_nowValue / _maxValue;
        float percent = (float.IsNaN(point)) ? 0 : point * 100f;

        GetImage((int)Images.Img_Exp).fillAmount = point;
        GetText((int)Texts.Text_Exp).text = $"{_nowValue}/{_maxValue} ({percent:F3})%";
    }
}