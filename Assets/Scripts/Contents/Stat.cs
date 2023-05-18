using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    protected int hp;
    protected int hpMax;
    protected float moveSpeed;
    protected int attackPower;
    protected float eatRange;
    protected int defense;

    public int Hp { get => hp; set => hp = value; }
    public int HpMax { get => hpMax; set => hpMax = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public int AttackPower { get => attackPower; set => attackPower = value; }
    public float EatRange { get => eatRange; set => eatRange = value; }
    public int Defense { get => defense; set => defense = value; }

    private void Start()
    {
        hp = 5;
        hpMax = hp;
        moveSpeed = 1f;
        attackPower = 1;
        eatRange = 1f;
    }

    public virtual void OnAttacked(Stat attacker)
    {
        int damage = Mathf.Max(0, attacker.attackPower - Defense);
        Hp -= damage;
        if (Hp <= 0)
        {
            Hp = 0;
            Debug.Log("À¸¾Ó Á×À½");
            OnDead(attacker);
        }
    }

    protected virtual void OnDead(Stat attacker)
    {
        ChickenStat chickenStat = attacker as ChickenStat;
        if(chickenStat != null)
        {
            chickenStat.GetComponent<Brain>().feedsFound++;
        }
        Destroy(gameObject);
    }
}