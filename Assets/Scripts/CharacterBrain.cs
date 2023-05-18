using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBrain : MonoBehaviour
{
    protected Vector3 destPos;          // 목표 지점
    protected GameObject targetObj;     // 목표 오브젝트

    [Header("캐릭터 상태")]
    [SerializeField] protected Define.CharacterState state = Define.CharacterState.Idle;

    public virtual Define.CharacterState State
    {
        get => state;
        set
        {
            Animator anim = GetComponent<Animator>();
            state = value;
            switch(state)
            {
                case Define.CharacterState.Idle: anim.CrossFade("WAIT", 0.1f); break;
                case Define.CharacterState.Moving: anim.CrossFade("WALK", 0.1f); break;
                case Define.CharacterState.Eat: anim.CrossFade("EAT", 0.1f); break;
            }
        }
    }

    private void Start()
    {
        Init();
    }

    protected virtual void Update()
    {
        switch (State)
        {
            case Define.CharacterState.Idle: UpdateIdle(); break;
            case Define.CharacterState.Moving: UpdateMoving(); break;
            case Define.CharacterState.Eat: UpdateAttack(); break;
        }
    }

    public abstract void Init();

    protected virtual void UpdateDie() { }

    protected virtual void UpdateIdle() { }

    protected virtual void UpdateMoving() { }

    protected virtual void UpdateAttack() { }
}