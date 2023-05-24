using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChickensBrain : CharacterBrain
{
    private FieldOfView sight;          // 시야각

    public int feedsFound = 0;

    public ChickensDNA DNA { private set; get; }        // 유전자 DNA


    public override void Init()
    {
        sight = GetComponent<FieldOfView>();

        DNA = new ChickensDNA();
        switch(DNA.WorldObjectType)
        {
            case Define.WorldObject.Chick: DNA.SetStatus(GetComponent<ChickenStat>(), Define.ChickenType.Chick); break;
            case Define.WorldObject.Chicken: DNA.SetStatus(GetComponent<ChickenStat>(), Define.ChickenType.Chicken); break;
        }

        if(GetComponentInChildren<UI_Hpbar>() == null)
        {
            Managers.UI.MakeWordSpaceUI<UI_Hpbar>(transform);
        }
    }

    protected override void UpdateIdle()
    {
        if (sight != null && sight.FindClosetObject() != null)
        {
            targetObj = sight.FindClosetObject();
            State = Define.CharacterState.Moving;
            return;
        }

        if (Random.Range(0f, 1f) < 0.25f)
        {
            // TODO: 이동할 위치가 원 밖이면 반복
            Vector3 randPos = transform.position + Random.insideUnitSphere * 1.5f;
            randPos.y = 0f;

            while (true)
            {
                Vector3 centerVec = randPos - Vector3.zero;
                float centerDistance = centerVec.magnitude;
                if (centerDistance > 5f)
                {
                    randPos = transform.position + Random.insideUnitSphere * 1.5f;
                    randPos.y = 0f;
                }
                else break;
            }

            destPos = randPos;
            State = Define.CharacterState.Moving;
            return;
        }
    }

    protected override void UpdateMoving()
    {
        if (targetObj != null)
        {
            destPos = targetObj.transform.position;
            float distance = (destPos - transform.position).magnitude;
            if (distance <= (float)DNA.StatusCode.Stats[StatType.EatRange])
            {
                if (targetObj.CompareTag("Feed"))
                {
                    State = Define.CharacterState.Eat;
                    sight.DebugMode = false;
                    return;
                }
            }
        }

        Vector3 dir = destPos - transform.position;
        dir.y = 0;

        if (dir.magnitude < 0.1f) State = Define.CharacterState.Idle;
        else
        {
            float moveDist = Mathf.Clamp((float)DNA.StatusCode.Stats[StatType.MoveSpeed] * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }
    }

    protected override void UpdateEat()
    {
        if (targetObj == null)
        {
            State = Define.CharacterState.Idle;
            sight.DebugMode = true;
            return;
        }

        if (targetObj != null)
        {
            Vector3 dir = targetObj.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20f * Time.deltaTime);
        }
    }


    //private void Update()
    //{
    //if(fov != null && fov.FindClosetObject() != null)
    //{
    //    transform.LookAt(fov.FindClosetObject().transform);
    //}
    //seeFeed = (false, false, false);
    //bool left = false;
    //bool front = false;
    //bool right = false;
    //canMove = true;

    //RaycastHit hit;
    //Debug.DrawRay(eyes.transform.position, eyes.transform.forward * 1f, Color.red);

    //if(Physics.SphereCast(eyes.transform.position, 0.1f, eyes.transform.forward, out hit, 1f, ~ignore))
    //{
    //    if(hit.collider.gameObject.CompareTag("Feed"))
    //    {
    //        front = true;
    //        canMove = false;
    //    }
    //}

    //if (Physics.SphereCast(eyes.transform.position, 0.1f, eyes.transform.right, out hit, 1f, ~ignore))
    //{
    //    if (hit.collider.gameObject.CompareTag("Feed"))
    //    {
    //        right = true;
    //    }
    //}

    //if (Physics.SphereCast(eyes.transform.position, 0.1f, -eyes.transform.right, out hit, 1f, ~ignore))
    //{
    //    if (hit.collider.gameObject.CompareTag("Feed"))
    //    {
    //        left = true;
    //    }
    //}
    //seeFeed = (left, front, right);
    //}

    private void FixedUpdate()
    {
        //transform.Rotate(0, dna.genes[seeFeed], 0);
        //if(canMove)
        //{
        //    transform.Translate(0f, 0f, 0.1f);
        //}
    }

    #region Event Callback
    private void OnEatEvent()
    {
        if (targetObj != null)
        {
            if (targetObj.CompareTag("Feed"))
            {
                FeedStat feedStat = targetObj.GetComponent<FeedStat>();
                feedStat.OnAttacked(DNA.StatusCode);
            }
        }
        State = Define.CharacterState.Eat;
    }
    #endregion
}