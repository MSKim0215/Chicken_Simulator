using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenStat : Stat
{


    private void Start()
    {
        hp = 5;
        hpMax = hp;
        moveSpeed = 1f;
        attackPower = 1;
        eatRange = 0.13f;
        defense = 0;
    }
}