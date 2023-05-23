using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager
{
    [Header("�ùķ��̼� ����")]
    [Range(25, 50)][SerializeField] private int beginPopulationSize;  // �ʱ� ���� �α���
    [Range(10f, 60f)][SerializeField] private float trailTime;        // ���� �ֱ�
    [Range(1f, 3f)][SerializeField] private float timeScale;
    public static float elapsed = 0f;       // ��� �ð�

    public List<GameObject> CurrGeneraionList { private set; get; } = new List<GameObject>();    // ���� ����
    public int Generation { private set; get; } = 1;     // ����

    public void Init()
    {
        beginPopulationSize = 25;
        trailTime = 60f;
        timeScale = 1f;
    }

    public void SpawnChick()
    {
        for (int i = 0; i < beginPopulationSize; i++)
        {
            GameObject chick = Managers.Resource.Instantiate("Unit/Chick");
            chick.transform.position = RandomSpawnPoint();
            chick.transform.rotation = RandomSpawnRotate();
            CurrGeneraionList.Add(chick);
        }
    }

    public void SpawnFeed()
    {
        for (int i = 0; i < beginPopulationSize; i++)
        {
            GameObject feed = Managers.Resource.Instantiate("Unit/Feed");
            feed.transform.position = RandomSpawnPoint();
            feed.transform.position = new Vector3(feed.transform.position.x, 0.05f, feed.transform.position.z);
            feed.transform.rotation = RandomSpawnRotate();
        }
    }

    public void OnUpdate()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= trailTime)
        {
            Debug.Log("������");
            //BreedNewPopulation();
            elapsed = 0;
            SpawnFeed();
        }
    }

    /// <summary>
    /// �ڽ��� �����ϴ� �Լ�
    /// fother: �ƺ�
    /// mother: ����
    /// </summary>
    //private GameObject Breed(GameObject fother, GameObject mother)
    //{
    //    GameObject offsetSpring = GetOffset(fother.tag);
    //    Brain brain = offsetSpring.GetComponent<Brain>();
    //    if (Random.Range(0, 100) == 1)
    //    {   // �������� ����, 1% Ȯ��
    //        brain.Init();
    //        Debug.LogWarning($"����Ʈ �߻�! {offsetSpring.name}");
    //    }
    //    else
    //    {
    //        brain.Init();
    //        brain.dna.CombineGenes(fother.GetComponent<Brain>().dna, mother.GetComponent<Brain>().dna);
    //    }
    //    return offsetSpring;
    //}

    //private GameObject GetOffset(string tag)
    //{
    //    switch(tag)
    //    {
    //        case "Chick": return Instantiate(chickPrefab, RandomSpawnPoint(), RandomSpawnRotate(), transform);
    //    }
    //    return null;
    //}

    /// <summary>
    /// ���ο� ������ ���Ƹ��� ���� �����ϴ� �Լ�
    /// </summary>
    //private void BreedNewPopulation()
    //{
    //    // ���̸� ���� ���� ���� ��ü ���ַ� ���� ��Ŵ
    //    List<GameObject> sortedList = currGeneraionList.OrderByDescending(o => o.GetComponent<Brain>().feedsFound).ToList();
    //    string feedCollected = $"Generation: {generation}";
    //    foreach (GameObject g in sortedList)
    //    {
    //        feedCollected += $", {g.GetComponent<Brain>().feedsFound}";
    //    }
    //    Debug.Log("Feeds: " + feedCollected);
    //    currGeneraionList.Clear();     // ���� ���� ����

    //    while (currGeneraionList.Count < beginPopulationSize)
    //    {
    //        int bestParentCutoff = sortedList.Count / 4;    // ���� 25% ���Ƹ��� �θ�� ����
    //        for (int i = 0; i < bestParentCutoff - 1; i++)
    //        {
    //            for (int j = 1; j < bestParentCutoff; j++)
    //            {   // ���� ���Ƹ����� DNA�� ���� �����Ͽ� ������ ����
    //                currGeneraionList.Add(Breed(sortedList[i], sortedList[j]));
    //                if (currGeneraionList.Count == beginPopulationSize) break;

    //                currGeneraionList.Add(Breed(sortedList[j], sortedList[i]));
    //                if (currGeneraionList.Count == beginPopulationSize) break;
    //            }
    //            if (currGeneraionList.Count == beginPopulationSize) break;
    //        }
    //    }

    //    for (int i = 0; i < sortedList.Count; i++)
    //    {
    //        Destroy(sortedList[i]);
    //    }
    //    generation++;
    //}

    private Vector3 RandomSpawnPoint()
    {
        Vector3 randRespawnVec = Random.insideUnitSphere * 5f;
        randRespawnVec.y = 0;
        return randRespawnVec;
    }

    private Quaternion RandomSpawnRotate()
    {
        return Quaternion.Euler(0, Random.Range(0, 360f), 0); 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Vector3.zero, 5f);
    }
}