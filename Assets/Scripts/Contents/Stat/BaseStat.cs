using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GenesStatType
{
    HpMax,
    MoveSpeed,
    EatPower,
    EatRange,
    Defense
}

public class BaseStat : MonoBehaviour
{
    protected int nowHp;        // 현재 체력량

    public Dictionary<GenesStatType, object> GenesStats { private set; get; } = new Dictionary<GenesStatType, object>();            // 유전 가능한 스탯

    public int NowHp
    {
        get => nowHp;
        set => nowHp = value;
    }

    public virtual void OnAttacked(BaseStat attacker)
    {
        int damage = Mathf.Max(0, (int)attacker.GenesStats[GenesStatType.EatPower] - (int)attacker.GenesStats[GenesStatType.Defense]);
        NowHp -= damage;
        if(NowHp <= 0)
        {
            NowHp = 0;
            OnDead(attacker);
        }
    }

    protected virtual void OnDead(BaseStat attacker)
    {
        ChickenStat chickenStat = attacker as ChickenStat;
        if (chickenStat != null)
        {
            ChickensBrain brain = chickenStat.GetComponent<ChickensBrain>();
            brain.eatingCount++;
            brain.ClearTarget();
            chickenStat.NowExp += 1;
        }
        Managers.Game.Despawn(gameObject);
    }
}