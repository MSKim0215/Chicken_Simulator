using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Flock : MonoBehaviour
{
    [Header("���� �ɼ�")]
    [SerializeField] private Define.WorldObject type;           // ������Ʈ Ÿ��
    [Range(2, 10)] public int startingCount = 2;                // ���� ��ü��
    [Range(0f, 10f)] public float findNeighborRadius = 1.5f;    // ��ü �̿� Ž�� �ݰ�
    [Range(1f, 5f)] public float agentDensity = 2f;             // ��ü �е�

    private List<BaseBrain> agents = new List<BaseBrain>();     // ��ü �迭
    private List<Transform> groups = new List<Transform>();     // ��ü �׷� �迭

    public float SquareNeighborRadius { private set; get; }

    private void Start()
    {
        SquareNeighborRadius = findNeighborRadius * findNeighborRadius;
    }

    public void Init(Define.WorldObject type)
    {
        this.type = type;
        MakeTable();
    }

    private void MakeTable()
    {
        string[] names = GetEnumNames();
        if (names == null) return;

        for (int i = 0; i < names.Length; i++)
        {
            GameObject temp = new GameObject { name = $"{names[i]}s" };
            temp.transform.parent = transform;
            groups.Add(temp.transform);
        }
    }

    private string[] GetEnumNames()
    {
        switch(type)
        {
            case Define.WorldObject.ChickGroup: return System.Enum.GetNames(typeof(Define.ChickenType));
        }
        return null;
    }

    public GameObject Spawn(string path)
    {
        Vector3 flockPos = transform.position;              // ���� ��ġ
        Vector3 randomOffset = RandomSpawnPoint();          // ������ �������� ���
        Vector3 agentPos = flockPos + randomOffset;         // ���� ��ġ

        GameObject prefab = Managers.Resource.Instantiate(path);
        prefab.transform.position = agentPos;
        prefab.transform.rotation = RandomSpawnRotate();
        SetParent(prefab);

        return prefab;
    }

    private void SetParent(GameObject prefab)
    {
        switch (type)
        {
            case Define.WorldObject.ChickGroup:
                {
                    ChickensBrain brain = prefab.GetComponent<ChickensBrain>();
                    brain.MakeDNA();

                    prefab.transform.parent = groups[(int)brain.DNA.Type];
                }
                break;
        }
    }

    public Vector3 RandomSpawnPoint()
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