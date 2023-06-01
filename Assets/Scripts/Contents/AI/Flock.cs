using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    [Header("���� �ɼ�")]
    [Range(2, 10)] public int startingCount = 2;                // ���� ��ü��
    [Range(0f, 10f)] public float findNeighborRadius = 1.5f;    // ��ü �̿� Ž�� �ݰ�
    [Range(1f, 5f)] public float agentDensity = 2f;             // ��ü �е�

    private List<BaseBrain> agents = new List<BaseBrain>();   // ��ü �迭

    public float SquareNeighborRadius { private set; get; }

    private void Start()
    {
        SquareNeighborRadius = findNeighborRadius * findNeighborRadius;
    }

    public GameObject Spawn(string path)
    {
        Vector3 flockPos = transform.position;              // ���� ��ġ
        Vector3 randomOffset = RandomSpawnPoint();          // ������ �������� ���
        Vector3 agentPos = flockPos + randomOffset;         // ���� ��ġ

        GameObject prefab = Managers.Resource.Instantiate(path, transform);
        prefab.transform.position = agentPos;
        prefab.transform.rotation = RandomSpawnRotate();

        agents.Add(prefab.GetComponent<BaseBrain>());

        return prefab;
    }

    private Vector3 RandomSpawnPoint()
    {
        Vector3 randRespawnVec = Random.insideUnitSphere * agentDensity;  // ������ �������� ���
        randRespawnVec.y = 0;
        return randRespawnVec;
    }

    private Quaternion RandomSpawnRotate() => Quaternion.Euler(0, Random.Range(0, 360f), 0);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, agentDensity);
    }
}