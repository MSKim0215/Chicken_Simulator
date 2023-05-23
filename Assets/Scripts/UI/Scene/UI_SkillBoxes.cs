using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillBoxes : UI_Scene
{
    private enum GameObjects
    {
        Base_SkillBox
    }

    public override void Init()
    {
        if (isInit) return;
        isInit = true;

        base.Init();

        Bind<GameObject>(typeof(GameObjects));

        // TODO: 사용자의 인벤토리 정보 세팅 진행
        for (int i = 0; i < 5; i++)
        {
            GameObject item = Managers.UI.MakeSubItem<UI_SkillBoxes_Box>(GetObject((int)GameObjects.Base_SkillBox).transform).gameObject;
            UI_SkillBoxes_Box skillBox = item.GetOrAddComponent<UI_SkillBoxes_Box>();
        }
    }
}