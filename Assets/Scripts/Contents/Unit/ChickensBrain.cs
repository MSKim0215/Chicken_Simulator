using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickensBrain : BehaviorBrain
{
    private bool isDelay = false;               // ������ ���� ���� üũ
    public bool isBreed = false;                // ���� ���� üũ (���밡 ��ȭ�ϸ� �ʱ�ȭ)

    public ChickensDNA DNA { private set; get; }            // ������ DNA

    public override void Init()
    {
        base.Init();

        AgentFlock = Managers.Game.ChickensGroupFlock;
    }

    /// <summary>
    /// �ű� DNA ����
    /// </summary>
    public void MakeDNA()
    {
        Define.ChickenType tempType = Define.ChickenType.None;
        switch(tag)
        {
            case "Chick": tempType = Define.ChickenType.Chick; break;
            case "Chicken": tempType = Define.ChickenType.Chicken; break;
        }

        DNA = new ChickensDNA();
        DNA.Init(GetComponent<ChickenStat>(), tempType);
    }

    /// <summary>
    /// ���� DNA ����
    /// </summary>
    public void CopyDNA(ChickensDNA targetDNA)
    {
        if (DNA == null) return;

        DNA.Init(targetDNA.StatusCode);
    }

    protected override void UpdateIdle()
    {
        if (isDelay) return;
        if (FOV == null) return;

        if (FOV.FindClosetObject() != null)
        {   // TODO: �þ� ���� �ȿ� ��ǥ���� �ִٸ� �̵� ���·� ����
            Target = FOV.FindClosetObject();
            State = Define.CharacterState.Moving;
            return;
        }

        if (Random.Range(0, 101) < 50)
        {   // TODO: 50% Ȯ���� ���� �̵� ����
            Vector3 randPosition = transform.position + Managers.Game.ChickensGroupFlock.RandomSpawnPoint();

            while (true)
            {
                Vector3 centerVec = randPosition - Vector3.zero;
                float centerDistance = centerVec.magnitude;
                if (centerDistance > Managers.Game.ChickensGroupFlock.agentDensity)
                {
                    randPosition = transform.position + Managers.Game.ChickensGroupFlock.RandomSpawnPoint();
                    randPosition.y = 0f;
                }
                else break;
            }

            destPosition = randPosition;
            State = Define.CharacterState.Moving;
            return;
        }
        else
        {
            if (!isDelay)
            {
                StartCoroutine(DelayTime());
            }
        }
    }

    private IEnumerator DelayTime()
    {
        isDelay = true;
        yield return new WaitForSeconds(1f);
        isDelay = false;
    }

    protected override void UpdateMoving()
    {
        if (Target != null)
        {   // TODO: ��ǥ���� �ִ� �̵��� ���
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
        if (DNA.Level >= Managers.Data.ChickenGroupLevelTable[Define.TableLabel].ChickGrowMax) return true;
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