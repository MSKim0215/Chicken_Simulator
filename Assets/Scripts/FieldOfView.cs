using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Header("�þ߰� �ɼ�")]
    [SerializeField] private bool debugMode = true;
    [Range(0f, 360f)][SerializeField] private float ViewAngle = 0f;
    [SerializeField] private float ViewRadius = 1f;
    [SerializeField] private LayerMask TargetMask;
    [SerializeField] private LayerMask ObstacleMask;
    [SerializeField] private float height;

    private List<Collider> targetList = new List<Collider>();

    public bool DebugMode { set => debugMode = value; }

    private void OnDrawGizmos()
    {
        if (!debugMode) return;

        Vector3 myPos = transform.position + Vector3.up * height;
        Gizmos.DrawWireSphere(myPos, ViewRadius);
        Gizmos.color = Color.gray;

        float lookingAngle = transform.eulerAngles.y;       // ĳ���Ͱ� �ٶ󺸴� ������ ����
        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + ViewAngle * 0.5f);
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - ViewAngle * 0.5f);
        Vector3 lookDir = AngleToDir(lookingAngle);

        Debug.DrawRay(myPos, rightDir * ViewRadius, Color.blue);
        Debug.DrawRay(myPos, leftDir * ViewRadius, Color.blue);
        Debug.DrawRay(myPos, lookDir * ViewRadius, Color.cyan);

        targetList.Clear();
        Collider[] targets = Physics.OverlapSphere(myPos, ViewRadius, TargetMask);
        if (targets.Length == 0) return;

        foreach (Collider target in targets)
        {
            Renderer targetRenderer = target.GetComponent<Renderer>();
            if (targetRenderer == null) continue;  // Renderer ������Ʈ�� ���� ��� �ǳʶݴϴ�.

            Bounds targetBounds = targetRenderer.bounds;
            Vector3 closestPoint = targetBounds.ClosestPoint(myPos);  // Ž�� ���� ������ ���� ����� ����Ʈ�� ã���ϴ�.
            Vector3 targetDir = (closestPoint - myPos).normalized;
            float targetAngle = Mathf.Abs(Mathf.DeltaAngle(Vector3.Angle(lookDir, targetDir), 0f));

            if (targetAngle <= ViewAngle && !Physics.Raycast(myPos, targetDir, ViewRadius, ObstacleMask))
            {
                targetList.Add(target);
                if (debugMode) Debug.DrawLine(myPos, closestPoint, Color.red);
            }
        }
    }

    private Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    }

    public GameObject FindClosetObject()
    {
        if (targetList.Count == 0) return null;

        GameObject closetObject = null;
        float closetDistance = Mathf.Infinity;
        Vector3 centerPoint = transform.position;

        foreach(Collider target in targetList)
        {
            if (target == null) continue;

            Vector3 closestPoint = target.ClosestPoint(centerPoint);    // ������ ���� ����� ����Ʈ ȹ��
            float distance = Vector3.Distance(centerPoint, closestPoint);
            if(distance < closetDistance)
            {
                closetObject = target.gameObject;
                closetDistance = distance;
            }
        }
        return closetObject;
    }
}