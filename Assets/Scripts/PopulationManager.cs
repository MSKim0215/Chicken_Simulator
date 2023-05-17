using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopulationManager : MonoBehaviour
{
    public GameObject botPrefab;        // ���Ƹ� ������
    public GameObject[] startingPos;    // ������ ���� �迭
    public int populationSize = 50;     // �ʱ� �α���
    public float trailTime = 10f;       // ���� �ֱ�
    public float timeScale = 2f;        
    public static float elapsed = 0;    // ��� �ð�

    private List<GameObject> population = new List<GameObject>();   // ���� ���� �迭
    private int generation = 1;     // ����

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
    /// �ڽ��� �����ϴ� �Լ�
    /// fother: �ƺ�
    /// mother: ����
    /// </summary>
    private GameObject Breed(GameObject fother, GameObject mother)
    {
        int index = Random.Range(0, startingPos.Length);
        GameObject offsetSpring = Instantiate(botPrefab, startingPos[index].transform.position, transform.rotation, transform);
        offsetSpring.transform.Rotate(0, Mathf.Round(Random.Range(-90, 91) / 90) * 90, 0);
        Brain brain = offsetSpring.GetComponent<Brain>();
        if(Random.Range(0, 100) == 1)
        {   // �������� ����, 1% Ȯ��
            brain.Init();
            Debug.LogWarning($"����Ʈ �߻�! {offsetSpring.name}");
        }
        else
        {
            brain.Init();
            brain.dna.Combine(fother.GetComponent<Brain>().dna, mother.GetComponent<Brain>().dna);
        }
        return offsetSpring;
    }

    /// <summary>
    /// ���ο� ������ ���Ƹ��� ���� �����ϴ� �Լ�
    /// </summary>
    private void BreedNewPopulation()
    {
        // ���̸� ���� ���� ���� ��ü ���ַ� ���� ��Ŵ
        List<GameObject> sortedList = population.OrderByDescending(o => o.GetComponent<Brain>().feedsFound).ToList();
        string feedCollected = $"Generation: {generation}";
        foreach(GameObject g in sortedList)
        {
            feedCollected += $", {g.GetComponent<Brain>().feedsFound}";
        }
        Debug.Log("Feeds: " + feedCollected);
        population.Clear();     // ���� ���� ����

        while (population.Count < populationSize)
        {
            int bestParentCutoff = sortedList.Count / 4;    // ���� 25% ���Ƹ��� �θ�� ����
            for (int i = 0; i < bestParentCutoff - 1; i++)
            {
                for(int j = 1; j < bestParentCutoff; j++)
                {   // ���� ���Ƹ����� DNA�� ���� �����Ͽ� ������ ����
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
            Debug.Log("������");
            BreedNewPopulation();
            elapsed = 0;
        }
    }
}