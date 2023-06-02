using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorBrain : BaseBrain
{   // BaseBrain�� ��� ���� �������� �ִ� ������Ʈ�� ��Ʈ�ѷ�
    protected Vector3 destPosition;     // �̵� ��ǥ ����

    public int eatingCount = 0;         // ���̸� ���� Ƚ��

    public FieldOfView FOV { protected set; get; }          // �þ߰�
    public GameObject Target { protected set; get; }        // ��ǥ��

    public override void Init()
    {
        base.Init();

        FOV = GetComponent<FieldOfView>();
    }

    public void ClearTarget() => Target = null;
}