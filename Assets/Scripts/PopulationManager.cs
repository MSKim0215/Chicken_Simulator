using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopulationManager : MonoBehaviour
{
    public GameObject botPrefab;        // 병아리 프리팹
    public GameObject[] startingPos;    // 리스폰 지역 배열
    public int populationSize = 50;     // 초기 인구수
    public float trailTime = 10f;       // 세대 주기
    public float timeScale = 2f;        
    public static float elapsed = 0;    // 경과 시간

    private List<GameObject> population = new List<GameObject>();   // 현재 세대 배열
    private int generation = 1;     // 세대

    GUIStyle guiStyle = new GUIStyle();
    private void OnGUI()
    {
        guiStyle.fontSize = 25;
        guiStyle.normal.textColor = Color.white;
        GUI.BeginGroup(new Rect(10, 10, 250, 250));
        GUI.Box(new Rect(0, 0, 140, 140), "Stats", guiStyle);
        GUI.Label(new Rect(10, 25, 200, 30), "Gen: " + generation, guiStyle);
        GUI.Label(new Rect(10, 50, 200, 30), string.Format("Time: {0:0.00}", elapsed), guiStyle);
        GUI.Label(new Rect(10, 75, 200, 30), "Population: " + population.Count, guiStyle);
        GUI.EndGroup();
    }

    private void Start()
    {
        for(int i = 0; i < populationSize; i++)
        {
            int index = Random.Range(0, startingPos.Length);
            GameObject bot = Instantiate(botPrefab, startingPos[index].transform.position, transform.rotation, transform);
            bot.transform.Rotate(0, Mathf.Round(Random.Range(-90, 91) / 90) * 90, 0);
            bot.GetComponent<Brain>().Init();
            population.Add(bot);
        }
        Time.timeScale = timeScale;
    }

    /// <summary>
    /// 자식을 생성하는 함수
    /// fother: 아빠
    /// mother: 엄마
    /// </summary>
    private GameObject Breed(GameObject fother, GameObject mother)
    {
        int index = Random.Range(0, startingPos.Length);
        GameObject offsetSpring = Instantiate(botPrefab, startingPos[index].transform.position, transform.rotation, transform);
        offsetSpring.transform.Rotate(0, Mathf.Round(Random.Range(-90, 91) / 90) * 90, 0);
        Brain brain = offsetSpring.GetComponent<Brain>();
        if(Random.Range(0, 100) == 1)
        {   // 돌연변이 발현, 1% 확률
            brain.Init();
            Debug.LogWarning($"뮤턴트 발생! {offsetSpring.name}");
        }
        else
        {
            brain.Init();
            brain.dna.Combine(fother.GetComponent<Brain>().dna, mother.GetComponent<Brain>().dna);
        }
        return offsetSpring;
    }

    /// <summary>
    /// 새로운 세대의 병아리를 번식 생성하는 함수
    /// </summary>
    private void BreedNewPopulation()
    {
        // 먹이를 가장 많이 먹은 개체 위주로 번식 시킴
        List<GameObject> sortedList = population.OrderByDescending(o => o.GetComponent<Brain>().feedsFound).ToList();
        string feedCollected = $"Generation: {generation}";
        foreach(GameObject g in sortedList)
        {
            feedCollected += $", {g.GetComponent<Brain>().feedsFound}";
        }
        Debug.Log("Feeds: " + feedCollected);
        population.Clear();     // 기존 세대 제거

        while (population.Count < populationSize)
        {
            int bestParentCutoff = sortedList.Count / 4;    // 상위 25% 병아리만 부모로 선택
            for (int i = 0; i < bestParentCutoff - 1; i++)
            {
                for(int j = 1; j < bestParentCutoff; j++)
                {   // 상위 병아리들의 DNA를 교차 선택하여 번식을 진행
                    population.Add(Breed(sortedList[i], sortedList[j]));
                    if (population.Count == populationSize) break;

                    population.Add(Breed(sortedList[j], sortedList[i]));
                    if (population.Count == populationSize) break;
                }
                if(population.Count == populationSize) break;
            }
        }

        for(int i = 0; i < sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }
        generation++;
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        if(elapsed >= trailTime)
        {
            Debug.Log("끝남요");
            BreedNewPopulation();
            elapsed = 0;
        }
    }
}