using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickensBrain : BehaviorBrain
{
    private Define.ChickenType type;            // 종류

    public bool isBreed = false;                // 번식 유무 체크 (세대가 변화하면 초기화)

    public ChickensDNA DNA { private set; get; }            // 유전자 DNA

    public override void Init()
    {
        base.Init();

        switch(tag)
        {
            case "Chick": type = Define.ChickenType.Chick; break;
            case "Chicken": type = Define.ChickenType.Chicken; break;
        }

        DNA = new ChickensDNA();
        DNA.Init(GetComponent<ChickenStat>(), type);
    }

    protected override void UpdateIdle()
    {
        if (FOV == null) return;

        if(FOV.FindClosetObject() != null)
        {   // TODO: 시야 범위 안에 목표물이 있다면 이동 상태로 변경
            Target = FOV.FindClosetObject();
            State = Define.CharacterState.Moving;
            return;
        }

        if(Random.Range(0, 101) < 25)
        {   // TODO: 25% 확률로 자유 이동 실행
            Vector3 randPosition = transform.position + Random.insideUnitSphere * 1.5f;
            randPosition.y = 0f;

            while(true)
            {
                Vector3 centerVec = randPosition - Vector3.zero;
                float centerDistance = centerVec.magnitude;
                if (centerDistance > 5f)
                {
                    randPosition = transform.position + Random.insideUnitSphere * 1.5f;
                    randPosition.y = 0f;
                }
                else break;
            }

            destPosition = randPosition;
            State = Define.CharacterState.Moving;
            return;
        }
    }

    protected override void UpdateMoving()
    {
        if (Target != null)
        {   // TODO: 목표물이 있는 이동일 경우
            destPosition = Target.transform.position;
            float distance = (destPosition - transform.position).magnitude;
            if (distance <= (float)DNA.EatRange)
            {
                if (Target.CompareTag("Feed"))
                {
                    State = Define.CharacterState.Eat;
                    FOV.DebugMode = false;
                    return;
                }
            }
        }

        Vector3 dir = destPosition - transform.position;
        dir.y = 0;

        if (dir.magnitude < 0.1f) State = Define.CharacterState.Idle;
        else
        {
            float moveDist = Mathf.Clamp((float)DNA.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20f * Time.deltaTime);
        }
    }

    protected override void UpdateEat()
    {
        if (Target == null)
        {
            State = Define.CharacterState.Idle;
            FOV.DebugMode = true;
            return;
        }

        if (Target != null)
        {
            Vector3 dir = Target.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20f * Time.deltaTime);
        }
    }

    public bool CheckEvolutionAble()
    {
        if (!Managers.Data.ChickenStatDict.TryGetValue(DNA.Level + 1, out _)) return true;
        return false;
    }

    #region Event Callback
    private void OnEatEvent()
    {
        if (Target != null)
        {
            if (Target.CompareTag("Feed"))
            {
                FeedStat feedStat = Target.GetComponent<FeedStat>();
                feedStat.OnAttacked(DNA.StatusCode);
            }
        }
        State = Define.CharacterState.Eat;
    }
    #endregion
}