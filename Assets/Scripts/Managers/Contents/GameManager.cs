using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager
{
    [Header("시뮬레이션 세팅")]
    [Range(25, 50)][SerializeField] private int beginPopulationSize;  // 초기 시작 인구수
    [Range(10f, 60f)][SerializeField] private float trailTime;        // 세대 주기
    [Range(1f, 3f)][SerializeField] private float timeScale;
    public static float elapsed = 0f;       // 경과 시간

    public List<GameObject> CurrGeneraionList { private set; get; } = new List<GameObject>();    // 현재 세대
    public int Generation { private set; get; } = 1;     // 세대
    public int BeginPopulationSize => beginPopulationSize;

    private HashSet<GameObject> chicks = new HashSet<GameObject>();     // 병아리 집합
    private HashSet<GameObject> chickens = new HashSet<GameObject>();   // 닭 집합
    private HashSet<GameObject> feeds = new HashSet<GameObject>();      // 먹이 집합
    
    private Dictionary<bool, Material> chickMaterials = new Dictionary<bool, Material>();   // 돌연변이, 일반 병아리 머티리얼
    private Dictionary<bool, Material> chickenMaterials = new Dictionary<bool, Material>(); // 돌연변이, 일반 닭 머티리얼

    public Action<int> OnSpawnEvent;

    public void Init()
    {
        beginPopulationSize = 25;
        trailTime = 20f;
        timeScale = 2f;

        chickMaterials.Add(true, Managers.Resource.Load<Material>("Materials/Mutant Chick"));
        chickMaterials.Add(false, Managers.Resource.Load<Material>("Materials/Normal Chick"));

        chickenMaterials.Add(true, Managers.Resource.Load<Material>("Materials/Mutant Chicken"));
        chickenMaterials.Add(false, Managers.Resource.Load<Material>("Materials/Normal Chicken"));
    }

    public GameObject Spawn(Define.WorldObject type, string path, Transform parent = null, int spawnCount = 1)
    {
        GameObject prefab = Managers.Resource.Instantiate(path, parent);
        switch (type)
        {
            case Define.WorldObject.Chick:
                {
                    if (!chicks.Contains(prefab))
                    {
                        chicks.Add(prefab);
                        prefab.transform.position = RandomSpawnPoint();
                        prefab.transform.rotation = RandomSpawnRotate();
                        OnSpawnEvent?.Invoke(spawnCount);
                    }
                }
                break;

            case Define.WorldObject.Chicken:
                {
                    if(!chickens.Contains(prefab))
                    {
                        chickens.Add(prefab);
                        prefab.transform.position = RandomSpawnPoint();
                        prefab.transform.rotation = RandomSpawnRotate();
                        OnSpawnEvent?.Invoke(spawnCount);
                    }
                }
                break;

            case Define.WorldObject.Feed:
                {
                    if (!feeds.Contains(prefab))
                    {
                        feeds.Add(prefab);
                        prefab.transform.position = RandomSpawnPoint();
                        prefab.transform.rotation = RandomSpawnRotate();
                        prefab.transform.position = new Vector3(prefab.transform.position.x, 0.05f, prefab.transform.position.z);
                        OnSpawnEvent?.Invoke(spawnCount);
                    }
                }
                break;
        }
        return prefab;
    }

    public GameObject Spawn(Define.WorldObject type, string path, Vector3 dir, Quaternion eulr, Transform parent = null, int spawnCount = 1)
    {
        GameObject prefab = Managers.Resource.Instantiate(path, parent);
        switch (type)
        {
            case Define.WorldObject.Chicken:
                {
                    if (!chickens.Contains(prefab))
                    {
                        prefab.transform.localPosition = dir;
                        prefab.transform.rotation = eulr;
                        chickens.Add(prefab);
                        OnSpawnEvent?.Invoke(spawnCount);
                    }
                }
                break;
        }
        return prefab;
    }

    private Define.WorldObject GetWorldObjectType(GameObject target)
    {
        if(target.tag == "Feed")
        {
            return target.GetComponent<FeedBrain>().DNA.WorldObjectType;
        }
        else if(target.tag == "Chick" || target.tag == "Chicken")
        {
            return target.GetComponent<ChickensBrain>().DNA.WorldObjectType;
        }
        return Define.WorldObject.Unknown;
    }

    public void Despawn(GameObject target, int despawnCount = -1)
    {
        switch(GetWorldObjectType(target))
        {
            case Define.WorldObject.Chick:
                {
                    if (chicks.Contains(target))
                    {
                        chicks.Remove(target);
                        OnSpawnEvent?.Invoke(despawnCount);
                    }
                }
                break;

            case Define.WorldObject.Chicken:
                {
                    if (chickens.Contains(target))
                    {
                        chickens.Remove(target);
                        OnSpawnEvent?.Invoke(despawnCount);
                    }
                }
                break;

            case Define.WorldObject.Feed:
                {
                    if (feeds.Contains(target))
                    {
                        feeds.Remove(target);
                        OnSpawnEvent?.Invoke(despawnCount);
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

    public void SpawnChicken(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject chicken = SpawnChicken();
            CurrGeneraionList.Add(chicken);
        }
    }

    private GameObject SpawnChicken()
    {
        GameObject chicken = Managers.Resource.Instantiate("Unit/Chicken");
        chicken.transform.position = RandomSpawnPoint();
        chicken.transform.rotation = RandomSpawnRotate();
        return chicken;
    }

    public void Evolution()
    {
        List<GameObject> chicks = GetEvolutionAble();
        int count = chicks.Count;
        if (count == 0) return;

        GameObject chickenFlock = GameObject.Find("ChickenFlock");
        if (chickenFlock == null)
        {
            chickenFlock = new GameObject { name = "ChickenFlock" };
        }

        for (int i = 0; i < count; i++)
        {
            Spawn(Define.WorldObject.Chicken, "Unit/Chicken", chicks[i].transform.localPosition, chicks[i].transform.rotation, chickenFlock.transform);
        }

        for (int i = 0; i < count; i++)
        {
            Despawn(chicks[i]);
        }
    }

    private List<GameObject> GetEvolutionAble()
    {
        List<GameObject> result = new List<GameObject>();
        foreach(GameObject chick in chicks)
        {
            ChickenStat stat = chick.GetComponent<ChickenStat>();
            if(!Managers.Data.ChickStatDict.TryGetValue((int)stat.Stats[StatType.Level] + 1, out _))
            {
                Debug.Log("최대 레벨 달성");
                result.Add(chick);
            }
        }
        return result;
    }

    public void OnUpdate()
    {
        //elapsed += Time.deltaTime;
        //if (elapsed >= trailTime)
        //{
        //    Debug.Log("끝남요");
        //    BreedNewPopulation();
        //    elapsed = 0;
        //    SpawnFeed();
        //}
    }

    /// <summary>
    /// 자식을 생성하는 함수
    /// fother: 아빠
    /// mother: 엄마
    /// </summary>
    private GameObject Breed(GameObject fother, GameObject mother)
    {
        GameObject offsetSpring = GetOffset(fother.tag);
        ChickensBrain brain = offsetSpring.GetComponent<ChickensBrain>();
        brain.Init();

        if (UnityEngine.Random.Range(0, 100) < 30)
        {   // 돌연변이 발현, 1% 확률
            brain.DNA.isMutant = true;

            SkinnedMeshRenderer skin;

            if(brain.DNA.WorldObjectType == Define.WorldObject.Chick)
            {
                skin = offsetSpring.transform.Find("Toon Chick").GetComponent<SkinnedMeshRenderer>();
                Material[] mats = skin.materials;
                mats[0] = chickMaterials[brain.DNA.isMutant];
                skin.materials = mats;
            }
            else if(brain.DNA.WorldObjectType == Define.WorldObject.Chicken)
            {
                skin = offsetSpring.transform.Find("Toon Chicken").GetComponent<SkinnedMeshRenderer>();
                Material[] mats = skin.materials;
                mats[0] = chickenMaterials[brain.DNA.isMutant];
                skin.materials = mats;
            }
            
            Debug.LogWarning($"뮤턴트 발생! {offsetSpring.name + offsetSpring.transform.GetSiblingIndex()}");
        }
        else
        {
            brain.DNA.CombineGenes(fother.GetComponent<ChickensBrain>().DNA, mother.GetComponent<ChickensBrain>().DNA);
        }
        return offsetSpring;
    }

    private GameObject GetOffset(string tag)
    {
        switch (tag)
        {
            case "Chick": return SpawnChick();
            case "Chicken": return SpawnChicken();
        }
        return null;
    }

    /// <summary>
    /// 새로운 세대의 병아리를 번식 생성하는 함수
    /// </summary>
    public void BreedNewPopulation()
    {
        // 먹이를 가장 많이 먹은 개체 위주로 번식 시킴
        List<GameObject> sortedList = CurrGeneraionList.OrderByDescending(o => o.GetComponent<ChickensBrain>().feedsFound).ToList();
        string feedCollected = $"Generation: {Generation}";
        //foreach (GameObject g in sortedList)
        //{
        //    feedCollected += $", {g.GetComponent<ChickensBrain>().feedsFound}";
        //}
        Debug.Log("Feeds: " + feedCollected);
        CurrGeneraionList.Clear();     // 기존 세대 제거

        while (CurrGeneraionList.Count < beginPopulationSize)
        {
            int bestParentCutoff = sortedList.Count / 4;    // 상위 25% 병아리만 부모로 선택
            for (int i = 0; i < bestParentCutoff - 1; i++)
            {
                for (int j = 1; j < bestParentCutoff; j++)
                {   // 상위 병아리들의 DNA를 교차 선택하여 번식을 진행
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