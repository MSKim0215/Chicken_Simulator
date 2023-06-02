using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBrain : MonoBehaviour
{   // ��� ������Ʈ�� ��Ʈ�ѷ� (Feed, Egg, Chick, Chicken, Fox)
    [Header("���� ����")]
    [SerializeField] protected Define.CharacterState state = Define.CharacterState.Idle;

    public virtual Define.CharacterState State
    {
        get => state;
        set
        {
            Animator anim = GetComponent<Animator>();
            if (anim == null) return;

            state = value;
            switch (state)
            {
                case Define.CharacterState.Idle: anim.CrossFade("WAIT", 0.1f); break;
                case Define.CharacterState.Moving: anim.CrossFade("WALK", 0.1f); break;
                case Define.CharacterState.Eat: anim.CrossFade("EAT", 0.1f); break;
            }
        }
    }

    public Flock AgentFlock { protected set; get; }         // �Ҽ� ����
    public Collider AgentCollider {  protected set; get; }  // �ݶ��̴�

    private void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        AgentCollider = GetComponent<Collider>();
    }

    protected virtual void Update()
    {
        switch (State)
        {
            case Define.CharacterState.Idle: UpdateIdle(); break;
            case Define.CharacterState.Moving: UpdateMoving(); break;
            case Define.CharacterState.Eat: UpdateEat(); break;
            case Define.CharacterState.Die: UpdateDie(); break;
        }
    }

    protected virtual void UpdateIdle() { }

    protected virtual void UpdateMoving() { }

    protected virtual void UpdateEat() { }

    protected virtual void UpdateDie() { }
}