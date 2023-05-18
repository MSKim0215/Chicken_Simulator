using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedStat : Stat
{
    private void Start()
    {
        hp = 10;
        hpMax = hp;
        moveSpeed = 0f;
        attackPower = 0;
        eatRange = 0f;
    }
}