using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("시뮬레이션 세팅")]
    [Range(25, 50)][SerializeField] private int beginPopulationSize;  // 초기 시작 인구수
    [Range(10f, 60f)][SerializeField] private float trailTime;        // 세대 주기
    [Range(1f, 3f)][SerializeField] private float timeScale;
    public static float elapsed = 0f;       // 경과 시간

    public GameObject feedPrefab;
    public GameObject chickPrefab;

    [SerializeField] private List<GameObject> currGeneraionList = new List<GameObject>();    // 현재 세대
    [SerializeField] private int generation = 1;     // 세대

    GUIStyle guiStyle = new GUIStyle();
    private void OnGUI()
    {
        guiStyle.fontSize = 25;
        guiStyle.normal.textColor = Color.white;
        GUI.BeginGroup(new Rect(10, 10, 250, 250));
        GUI.Box(new Rect(0, 0, 140, 140), "목록", guiStyle);
        GUI.Label(new Rect(10, 25, 200, 30), "현재 세대: " + generation, guiStyle);
        GUI.Label(new Rect(10, 50, 200, 30), string.Format("세대 교체 시간: {0:0.00}", elapsed), guiStyle);
        GUI.Label(new Rect(10, 75, 200, 30), "인구수: " + currGeneraionList.Count, guiStyle);
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
            Debug.Log("끝남요");
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
    /// 자식을 생성하는 함수
    /// fother: 아빠
    /// mother: 엄마
    /// </summary>
    private GameObject Breed(GameObject fother, GameObject mother)
    {
        GameObject offsetSpring = GetOffset(fother.tag);
        Brain brain = offsetSpring.GetComponent<Brain>();
        if (Random.Range(0, 100) == 1)
        {   // 돌연변이 발현, 1% 확률
            brain.Init();
            Debug.LogWarning($"뮤턴트 발생! {offsetSpring.name}");
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
    /// 새로운 세대의 병아리를 번식 생성하는 함수
    /// </summary>
    private void BreedNewPopulation()
    {
        // 먹이를 가장 많이 먹은 개체 위주로 번식 시킴
        List<GameObject> sortedList = currGeneraionList.OrderByDescending(o => o.GetComponent<Brain>().feedsFound).ToList();
        string feedCollected = $"Generation: {generation}";
        foreach (GameObject g in sortedList)
        {
            feedCollected += $", {g.GetComponent<Brain>().feedsFound}";
        }
        Debug.Log("Feeds: " + feedCollected);
        currGeneraionList.Clear();     // 기존 세대 제거

        while (currGeneraionList.Count < beginPopulationSize)
        {
            int bestParentCutoff = sortedList.Count / 4;    // 상위 25% 병아리만 부모로 선택
            for (int i = 0; i < bestParentCutoff - 1; i++)
            {
                for (int j = 1; j < bestParentCutoff; j++)
                {   // 상위 병아리들의 DNA를 교차 선택하여 번식을 진행
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