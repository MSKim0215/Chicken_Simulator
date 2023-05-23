using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillBoxes_Box : UI_Base
{
    private enum Images
    {
        Img_Icon
    }

    private enum GameObjects
    {
        BoxOutline
    }

    public override void Init()
    {
        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));
    }
}