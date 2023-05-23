using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitFrame : UI_Scene
{
    private enum Images
    {
        Img_Hp,
        Img_Mp
    }

    private enum Texts
    {
        Text_Level,
        Text_Hp_Point, Text_Hp_Percent,
        Text_Mp_Point, Text_Mp_Percent
    }

    public override void Init()
    {
        if (isInit) return;
        isInit = true;

        base.Init();

        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));

        //// TODO: 사용자의 인벤토리 정보 세팅 진행
        //for(int i = 0; i < 8; i++)
        //{
        //    GameObject item = Managers.UI.MakeSubItem<UI_Inven_Item>(GetObject((int)GameObjects.ItemBag).transform).gameObject;
        //    UI_Inven_Item invenItem = item.GetOrAddComponent<UI_Inven_Item>();
        //    invenItem.SetInfo($"집행검 +{i}");
        //    invenItem.Init();
        //}
    }

    public void SetLevel(int _level)
    {
        if (!isInit) Init();
        GetText((int)Texts.Text_Level).text = _level.ToString();
    }

    public void SetHp(int _nowValue, int _maxValue)
    {
        if (!isInit) Init();

        float point = (float)_nowValue / _maxValue;
        int percent = (int)(point * 100);

        GetImage((int)Images.Img_Hp).fillAmount = point;
        GetText((int)Texts.Text_Hp_Point).text = $"{_nowValue}/{_maxValue}";
        GetText((int)Texts.Text_Hp_Percent).text = $"{percent}%";
    }

    public void SetMp(int _nowValue, int _maxValue)
    {
        if (!isInit) Init();

        float point = (float)_nowValue / _maxValue;
        int percent = (int)(point * 100);

        GetImage((int)Images.Img_Mp).fillAmount = point;
        GetText((int)Texts.Text_Mp_Point).text = $"{_nowValue}/{_maxValue}";
        GetText((int)Texts.Text_Mp_Percent).text = $"{percent}%";
    }
}