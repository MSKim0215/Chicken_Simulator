using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Flock : MonoBehaviour
{
    [Header("군집 옵션")]
    [SerializeField] private Define.WorldObject type;           // 오브젝트 타입
    [Range(2, 10)] public int startingCount = 2;                // 시작 개체값
    [Range(0f, 10f)] public float findNeighborRadius = 1.5f;    // 개체 이웃 탐색 반경
    [Range(1f, 5f)] public float agentDensity = 2f;             // 개체 밀도

    private List<BaseBrain> agents = new List<BaseBrain>();     // 개체 배열
    private List<Transform> groups = new List<Transform>();     // 개체 그룹 배열

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
        Vector3 flockPos = transform.position;              // 군집 위치
        Vector3 randomOffset = RandomSpawnPoint();          // 무작위 오프셋을 계산
        Vector3 agentPos = flockPos + randomOffset;         // 최종 위치

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
        Vector3 randRespawnVec = Random.insideUnitSphere * agentDensity;  // 무작위 오프셋을 계산
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