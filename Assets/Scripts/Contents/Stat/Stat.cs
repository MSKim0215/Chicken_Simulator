using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    Hp,
    MoveSpeed,
    AttackPower,
    EatRange,
    Defense,

    // Chicken
    ExpMax,
    Level
}

public class Stat : MonoBehaviour
{
    protected Dictionary<StatType, object> stats = new Dictionary<StatType, object>();
    protected int hpMax;

    public Dictionary<StatType, object> Stats { get => stats; set => stats = value; }
    public int HpMax { get => hpMax; set => hpMax = value; }

    public virtual void Init(Define.ChickenType type = Define.ChickenType.None, int level = 1)
    {
        Stats.Add(StatType.Hp, 5);
        Stats.Add(StatType.MoveSpeed, 1f);
        Stats.Add(StatType.AttackPower, 1);
        Stats.Add(StatType.EatRange, 1f);
        Stats.Add(StatType.Defense, 1);

        HpMax = (int)Stats[StatType.Hp];
    }

    public virtual void OnAttacked(Stat attacker)
    {
        int damage = Mathf.Max(0, (int)attacker.Stats[StatType.AttackPower] - (int)Stats[StatType.Defense]);
        Stats[StatType.Hp] = (int)Stats[StatType.Hp] - damage;
        if ((int)Stats[StatType.Hp] <= 0)
        {
            Stats[StatType.Hp] = 0;
            OnDead(attacker);
        }
    }

    protected virtual void OnDead(Stat attacker)
    {
        ChickenStat chickenStat = attacker as ChickenStat;
        if(chickenStat != null)
        {
            ChickensBrain brain = chickenStat.GetComponent<ChickensBrain>();
            brain.eatingCount++;
            brain.ClearTarget();
            chickenStat.NowExp += 1;
        }
        Managers.Game.Despawn(gameObject);
    }
}