using System;
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
    public int BeginPopulationSize => beginPopulationSize;

    private HashSet<GameObject> chicks = new HashSet<GameObject>();     // ���Ƹ� ����
    private HashSet<GameObject> feeds = new HashSet<GameObject>();      // ���� ����
    private Dictionary<bool, Material> chickMaterials = new Dictionary<bool, Material>();   // ��������, �Ϲ� ���Ƹ� ��Ƽ����

    public Action<int> OnSpawnEvent;

    public void Init()
    {
        beginPopulationSize = 25;
        trailTime = 20f;
        timeScale = 2f;

        chickMaterials.Add(true, Managers.Resource.Load<Material>("Materials/Mutant Chick"));
        chickMaterials.Add(false, Managers.Resource.Load<Material>("Materials/Normal Chick"));
    }

    public GameObject Spawn(Define.WorldObject type, string path, Transform parent = null)
    {
        GameObject prefab = Managers.Resource.Instantiate(path, parent);
        prefab.transform.position = RandomSpawnPoint();
        prefab.transform.rotation = RandomSpawnRotate();

        switch (type)
        {
            case Define.WorldObject.Chick:
                {
                    chicks.Add(prefab);
                    OnSpawnEvent?.Invoke(1);
                }
                break;
            case Define.WorldObject.Feed:
                {
                    prefab.transform.position = new Vector3(prefab.transform.position.x, 0.05f, prefab.transform.position.z);
                    feeds.Add(prefab);
                    OnSpawnEvent?.Invoke(1);
                }
                break;
        }
        return prefab;
    }

    private Define.WorldObject GetWorldObjectType(GameObject target)
    {
        RootDNA root = target.GetComponent<RootDNA>();
        if (root) return Define.WorldObject.Unknown;
        return root.WorldObjectType;
    }

    public void Despawn(GameObject target)
    {
        switch(GetWorldObjectType(target))
        {
            case Define.WorldObject.Chick:
                {
                    if (chicks.Contains(target))
                    {
                        chicks.Remove(target);
                        OnSpawnEvent?.Invoke(-1);
                    }
                }
                break;
            case Define.WorldObject.Feed:
                {
                    if (feeds.Contains(target))
                    {
                        feeds.Remove(target);
                        OnSpawnEvent?.Invoke(-1);
                    }
                }
                break;
        }
        Managers.Resource.Destroy(target);
    }

    public void SpawnChick(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject chick = SpawnChick();
            CurrGeneraionList.Add(chick);
        }
    }

    private GameObject SpawnChick()
    {
        GameObject chick = Managers.Resource.Instantiate("Unit/Chick");
        chick.transform.position = RandomSpawnPoint();
        chick.transform.rotation = RandomSpawnRotate();
        return chick;
    }

    public void SpawnFeed(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject feed = SpawnFeed();
        }
    }

    private GameObject SpawnFeed()
    {
        GameObject feed = Managers.Resource.Instantiate("Unit/Feed");
        feed.transform.position = RandomSpawnPoint();
        feed.transform.position = new Vector3(feed.transform.position.x, 0.05f, feed.transform.position.z);
        feed.transform.rotation = RandomSpawnRotate();
        return feed;
    }

    public void OnUpdate()
    {
        //elapsed += Time.deltaTime;
        //if (elapsed >= trailTime)
        //{
        //    Debug.Log("������");
        //    BreedNewPopulation();
        //    elapsed = 0;
        //    SpawnFeed();
        //}
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
        brain.Init();

        if (UnityEngine.Random.Range(0, 100) < 30)
        {   // �������� ����, 1% Ȯ��
            brain.dna.isMutant = true;

            SkinnedMeshRenderer skin = offsetSpring.transform.Find("Toon Chick").GetComponent<SkinnedMeshRenderer>();            
            Material[] mats = skin.materials;
            mats[0] = chickMaterials[brain.dna.isMutant];
            skin.materials = mats;

            Debug.LogWarning($"����Ʈ �߻�! {offsetSpring.name + offsetSpring.transform.GetSiblingIndex()}");
        }
        else
        {
            brain.dna.CombineGenes(fother.GetComponent<Brain>().dna, mother.GetComponent<Brain>().dna);
        }
        return offsetSpring;
    }

    private GameObject GetOffset(string tag)
    {
        switch (tag)
        {
            case "Chick": return SpawnChick();
        }
        return null;
    }

    /// <summary>
    /// ���ο� ������ ���Ƹ��� ���� �����ϴ� �Լ�
    /// </summary>
    private void BreedNewPopulation()
    {
        // ���̸� ���� ���� ���� ��ü ���ַ� ���� ��Ŵ
        List<GameObject> sortedList = CurrGeneraionList.OrderByDescending(o => o.GetComponent<Brain>().feedsFound).ToList();
        string feedCollected = $"Generation: {Generation}";
        foreach (GameObject g in sortedList)
        {
            feedCollected += $", {g.GetComponent<Brain>().feedsFound}";
        }
        Debug.Log("Feeds: " + feedCollected);
        CurrGeneraionList.Clear();     // ���� ���� ����

        while (CurrGeneraionList.Count < beginPopulationSize)
        {
            int bestParentCutoff = sortedList.Count / 4;    // ���� 25% ���Ƹ��� �θ�� ����
            for (int i = 0; i < bestParentCutoff - 1; i++)
            {
                for (int j = 1; j < bestParentCutoff; j++)
                {   // ���� ���Ƹ����� DNA�� ���� �����Ͽ� ������ ����
                    CurrGeneraionList.Add(Breed(sortedList[i], sortedList[j]));
                    if (CurrGeneraionList.Count == beginPopulationSize) break;

                    CurrGeneraionList.Add(Breed(sortedList[j], sortedList[i]));
                    if (CurrGeneraionList.Count == beginPopulationSize) break;
                }
                if (CurrGeneraionList.Count == beginPopulationSize) break;
            }
        }

        for (int i = 0; i < sortedList.Count; i++)
        {
            Managers.Resource.Destroy(sortedList[i]);
        }
        Generation++;
    }

    private Vector3 RandomSpawnPoint()
    {
        Vector3 randRespawnVec = UnityEngine.Random.insideUnitSphere * 5f;
        randRespawnVec.y = 0;
        return randRespawnVec;
    }

    private Quaternion RandomSpawnRotate()
    {
        return Quaternion.Euler(0, UnityEngine.Random.Range(0, 360f), 0); 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Vector3.zero, 5f);
    }
}