using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Brain : CharacterBrain
{
    public DNA dna;
    public GameObject eyes;

    private FieldOfView fov;
    private LayerMask ignore = 6;
    private (bool left, bool forward, bool right) seeFeed;
    private bool canMove = false;

    public float feedsFound = 0;

    public override void Init()
    {
        dna = new DNA();
        dna.SetStat(GetComponent<ChickenStat>());
        fov = GetComponent<FieldOfView>();
    }

    protected override void UpdateIdle()
    {
        if(fov != null && fov.FindClosetObject() != null)
        {
            targetObj = fov.FindClosetObject();
            State = Define.CharacterState.Moving;
            return;
        }
    }

    protected override void UpdateMoving()
    {
        if(targetObj != null)
        {
            destPos = targetObj.transform.position;
            float distance = (destPos - transform.position).magnitude;
            if(distance <= dna.stat.EatRange)
            {
                if(targetObj.CompareTag("Feed"))
                {
                    Debug.Log("이따다키마스");
                    State = Define.CharacterState.Eat;
                    return;
                }
            }
        }

        Vector3 dir = destPos - transform.position;
        dir.y = 0;
        if (dir.magnitude < 0.1f) State = Define.CharacterState.Idle;
        else
        {
            float moveDist = Mathf.Clamp(1f * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }
    }

    protected override void UpdateEat()
    {
        if(targetObj == null)
        {
            State = Define.CharacterState.Idle;
            return;
        }

        if(targetObj != null)
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
        Debug.Log("음 맛있다~");

        if(targetObj != null)
        {
            if(targetObj.CompareTag("Feed"))
            {
                FeedStat feedStat = targetObj.GetComponent<FeedStat>();
                feedStat.OnAttacked(dna.stat);
            }
        }
        State = Define.CharacterState.Eat;
    }
    #endregion
}