using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Inven_Item : UI_Base
{
    private enum GameObjects
    {
        Img_Icon,
        Text_Name
    }

    private string name;

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        Get<GameObject>((int)GameObjects.Text_Name).GetComponent<Text>().text = name;
        Get<GameObject>((int)GameObjects.Img_Icon).AddUIEvent((PointerEventData _data) =>
        {
            Debug.Log($"아이템 클릭! {name}");
        });
    }

    public void SetInfo(string _name) => name = _name;
}