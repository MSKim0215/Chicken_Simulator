using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("�ùķ��̼� ����")]
    [Range(25, 50)][SerializeField] private int beginPopulationSize;  // �ʱ� ���� �α���
    [Range(10f, 60f)][SerializeField] private float trailTime;        // ���� �ֱ�
    [Range(1f, 3f)][SerializeField] private float timeScale;
    public static float elapsed = 0f;       // ��� �ð�

    public GameObject feedPrefab;
    public GameObject chickPrefab;

    [SerializeField] private List<GameObject> currGeneraionList = new List<GameObject>();    // ���� ����
    [SerializeField] private int generation = 1;     // ����

    GUIStyle guiStyle = new GUIStyle();
    private void OnGUI()
    {
        guiStyle.fontSize = 25;
        guiStyle.normal.textColor = Color.white;
        GUI.BeginGroup(new Rect(10, 10, 250, 250));
        GUI.Box(new Rect(0, 0, 140, 140), "���", guiStyle);
        GUI.Label(new Rect(10, 25, 200, 30), "���� ����: " + generation, guiStyle);
        GUI.Label(new Rect(10, 50, 200, 30), string.Format("���� ��ü �ð�: {0:0.00}", elapsed), guiStyle);
        GUI.Label(new Rect(10, 75, 200, 30), "�α���: " + currGeneraionList.Count, guiStyle);
        GUI.EndGroup();
    }

    private void Start()
    {
        for (int i = 0; i < beginPopulationSize; i++)
        {
            GameObject feed = Instantiate(feedPrefab, RandomSpawnPoint(), RandomSpawnRotate(), transform);
            feed.transform.position = new Vector3(feed.transform.position.x, 0.05f, feed.transform.position.z);
        }

        for (int i = 0; i < beginPopulationSize; i++)
        {
            GameObject chick = Instantiate(chickPrefab, RandomSpawnPoint(), RandomSpawnRotate(), transform);
            currGeneraionList.Add(chick);
        }
        Time.timeScale = timeScale;
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= trailTime)
        {
            Debug.Log("������");
            BreedNewPopulation();
            elapsed = 0;

            for (int i = 0; i < beginPopulationSize; i++)
            {
                GameObject feed = Instantiate(feedPrefab, RandomSpawnPoint(), RandomSpawnRotate(), transform);
                feed.transform.position = new Vector3(feed.transform.position.x, 0.05f, feed.transform.position.z);
            }
        }
    }

    /// <summary>
    /// �ڽ��� �����ϴ� �Լ�
    /// fother: �ƺ�
    /// mother: ����
    /// </summary>
    private GameObject Breed(GameObject fother, GameObject mother)
    {
        GameObject offsetSpring = GetOffset(fother.tag);
        Brain brain = offsetSpring.GetComponent<Brain>();
        if (Random.Range(0, 100) == 1)
        {   // �������� ����, 1% Ȯ��
            brain.Init();
            Debug.LogWarning($"����Ʈ �߻�! {offsetSpring.name}");
        }
        else
        {
            brain.Init();
            brain.dna.CombineGenes(fother.GetComponent<Brain>().dna, mother.GetComponent<Brain>().dna);
        }
        return offsetSpring;
    }

    private GameObject GetOffset(string tag)
    {
        switch(tag)
        {
            case "Chick": return Instantiate(chickPrefab, RandomSpawnPoint(), RandomSpawnRotate(), transform);
        }
        return null;
    }

    /// <summary>
    /// ���ο� ������ ���Ƹ��� ���� �����ϴ� �Լ�
    /// </summary>
    private void BreedNewPopulation()
    {
        // ���̸� ���� ���� ���� ��ü ���ַ� ���� ��Ŵ
        List<GameObject> sortedList = currGeneraionList.OrderByDescending(o => o.GetComponent<Brain>().feedsFound).ToList();
        string feedCollected = $"Generation: {generation}";
        foreach (GameObject g in sortedList)
        {
            feedCollected += $", {g.GetComponent<Brain>().feedsFound}";
        }
        Debug.Log("Feeds: " + feedCollected);
        currGeneraionList.Clear();     // ���� ���� ����

        while (currGeneraionList.Count < beginPopulationSize)
        {
            int bestParentCutoff = sortedList.Count / 4;    // ���� 25% ���Ƹ��� �θ�� ����
            for (int i = 0; i < bestParentCutoff - 1; i++)
            {
                for (int j = 1; j < bestParentCutoff; j++)
                {   // ���� ���Ƹ����� DNA�� ���� �����Ͽ� ������ ����
                    currGeneraionList.Add(Breed(sortedList[i], sortedList[j]));
                    if (currGeneraionList.Count == beginPopulationSize) break;

                    currGeneraionList.Add(Breed(sortedList[j], sortedList[i]));
                    if (currGeneraionList.Count == beginPopulationSize) break;
                }
                if (currGeneraionList.Count == beginPopulationSize) break;
            }
        }

        for (int i = 0; i < sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }
        generation++;
    }

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