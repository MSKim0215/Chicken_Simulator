using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorBrain : BaseBrain
{   // BaseBrain을 상속 받은 움직임이 있는 오브젝트의 컨트롤러
    protected Vector3 destPosition;     // 이동 목표 지점

    public int eatingCount = 0;         // 먹이를 먹은 횟수

    public FieldOfView FOV { protected set; get; }          // 시야각
    public GameObject Target { protected set; get; }        // 목표물

    public override void Init()
    {
        base.Init();

        FOV = GetComponent<FieldOfView>();
    }

    public void ClearTarget() => Target = null;
}