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

    private HashSet<GameObject> eggs = new HashSet<GameObject>();       // �� ����
    private HashSet<GameObject> chicks = new HashSet<GameObject>();     // ���Ƹ� ����
    private HashSet<GameObject> chickens = new HashSet<GameObject>();   // �� ����
    private HashSet<GameObject> feeds = new HashSet<GameObject>();      // ���� ����

    private Dictionary<bool, Material> eggMaterials = new Dictionary<bool, Material>();     // ��������, �Ϲ� �� ��Ƽ����
    private Dictionary<bool, Material> chickMaterials = new Dictionary<bool, Material>();   // ��������, �Ϲ� ���Ƹ� ��Ƽ����
    private Dictionary<bool, Material> chickenMaterials = new Dictionary<bool, Material>(); // ��������, �Ϲ� �� ��Ƽ����

    public Action<int> OnSpawnEvent;

    public void Init()
    {
        beginPopulationSize = 2;
        trailTime = 20f;
        timeScale = 2f;

        eggMaterials.Add(true, Managers.Resource.Load<Material>("Materials/Mutant Egg"));
        eggMaterials.Add(false, Managers.Resource.Load<Material>("Materials/Normal Egg"));

        chickMaterials.Add(true, Managers.Resource.Load<Material>("Materials/Mutant Chick"));
        chickMaterials.Add(false, Managers.Resource.Load<Material>("Materials/Normal Chick"));

        chickenMaterials.Add(true, Managers.Resource.Load<Material>("Materials/Mutant Chicken"));
        chickenMaterials.Add(false, Managers.Resource.Load<Material>("Materials/Normal Chicken"));
    }

    public GameObject Spawn(string path, Transform parent = null)
    {
        GameObject prefab = Managers.Resource.Instantiate(path, parent);
        switch(prefab.tag)
        {
            case "Chick":
                {
                    if (!chicks.Contains(prefab))
                    {
                        chicks.Add(prefab);
                        prefab.transform.position = RandomSpawnPoint();
                        prefab.transform.rotation = RandomSpawnRotate();
                        OnSpawnEvent?.Invoke(1);
                    }
                }
                break;

            case "Chicken":
                {
                    if (!chickens.Contains(prefab))
                    {
                        chickens.Add(prefab);
                        prefab.transform.position = RandomSpawnPoint();
                        prefab.transform.rotation = RandomSpawnRotate();
                        OnSpawnEvent?.Invoke(1);
                    }
                }
                break;

            case "Feed":
                {
                    if (!feeds.Contains(prefab))
                    {
                        feeds.Add(prefab);
                        prefab.transform.position = RandomSpawnPoint();
                        prefab.transform.rotation = RandomSpawnRotate();
                        prefab.transform.position = new Vector3(prefab.transform.position.x, 0.05f, prefab.transform.position.z);
                        OnSpawnEvent?.Invoke(1);
                    }
                }
                break;
        }
        return prefab;
    }

    public GameObject Spawn(string path, Vector3 dir, Quaternion eulr, Transform parent = null)
    {
        GameObject prefab = Managers.Resource.Instantiate(path, parent);
        switch (prefab.tag)
        {
            case "Egg":
                {
                    if (!eggs.Contains(prefab))
                    {
                        prefab.transform.localPosition = dir;
                        prefab.transform.rotation = eulr;
                        eggs.Add(prefab);
                        OnSpawnEvent?.Invoke(1);
                    }
                }
                break;

            case "Chick":
                {
                    if(!chicks.Contains(prefab))
                    {
                        prefab.transform.localPosition = dir;
                        prefab.transform.rotation = eulr;
                        chicks.Add(prefab);
                        OnSpawnEvent?.Invoke(1);
                    }
                }
                break;

            case "Chicken":
                {
                    if (!chickens.Contains(prefab))
                    {
                        prefab.transform.localPosition = dir;
                        prefab.transform.rotation = eulr;
                        chickens.Add(prefab);
                        OnSpawnEvent?.Invoke(1);
                    }
                }
                break;
        }
        return prefab;
    }

    public void Despawn(GameObject target)
    {
        switch(target.tag)
        {
            case "Egg":
                {
                    if(eggs.Contains(target))
                    {
                        eggs.Remove(target);
                        OnSpawnEvent?.Invoke(-1);
                    }
                }
                break;

            case "Chick":
                {
                    if (chicks.Contains(target))
                    {
                        chicks.Remove(target);
                        OnSpawnEvent?.Invoke(-1);
                    }
                }
                break;

            case "Chicken":
                {
                    if (chickens.Contains(target))
                    {
                        chickens.Remove(target);
                        OnSpawnEvent?.Invoke(-1);
                    }
                }
                break;

            case "Feed":
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

    public void Evolution()
    {
        if (chicks.Count == 0) return;      // ���Ƹ��� ���ٸ� ����

        List<GameObject> evolutionList = new List<GameObject>();       // ��ȭ �����
        foreach(GameObject chick in chicks)
        {
            ChickensBrain brain = chick.GetComponent<ChickensBrain>();
            if (brain.CheckEvolutionAble()) evolutionList.Add(chick);
        }

        int count = evolutionList.Count;
        if (count == 0) return;     // ��ȭ ������� ���ٸ� ����

        GameObject chickenFlock = GameObject.Find("ChickenFlock");
        if (chickenFlock == null)
        {
            chickenFlock = new GameObject { name = "ChickenFlock" };
        }

        for (int i = 0; i < count; i++)
        {
            ChickensBrain newBrain = Spawn("Unit/Chicken", evolutionList[i].transform.localPosition, evolutionList[i].transform.rotation, chickenFlock.transform).GetComponent<ChickensBrain>();
            newBrain.MakeDNA();
            newBrain.CopyDNA(evolutionList[i].GetComponent<ChickensBrain>().DNA);
        }

        for (int i = 0; i < count; i++)
        {
            Despawn(evolutionList[i]);
        }
    }

    /// <summary>
    /// ���ο� ������ ���Ƹ��� ���� �����ϴ� �Լ�
    /// </summary>
    public void BreedNewPopulation()
    {
        if (chickens.Count < 2) return;        // ������ ���� ���ٸ� ����

        // TODO: ���̸� ���� ���� ���� ��ü�鸸 ���� ����
        List<GameObject> sortedList = chickens.OrderByDescending(x => x.GetComponent<ChickensBrain>().eatingCount).ToList();
        string feedCollected = $"���� ����: {Generation}\n";
        foreach(GameObject chicken in sortedList)
        {
            feedCollected += $"{chicken.GetComponentInParent<ChickensBrain>().eatingCount}, ";
        }
        Debug.Log($"���̸� ���� ����: {feedCollected}");

        //CurrGeneraionList.Clear();     // ���� ���� ����

        int bestParentCutoff = (sortedList.Count / 4 < 2) ? 2 : sortedList.Count / 4;  // ���� 25% �߸� �θ�� ���õ�
        Debug.Log($"���� ���� ��: {bestParentCutoff}");  
        for (int i = 0; i < bestParentCutoff - 1; i++)
        {
            for (int j = i + 1; j < bestParentCutoff; j++)
            {   // ���� �ߵ��� DNA�� ���� �����Ͽ� ���� ����
                ChickensBrain fother = sortedList[i].GetComponent<ChickensBrain>();
                ChickensBrain mother = sortedList[j].GetComponent<ChickensBrain>();

                if(!fother.isBreed && !mother.isBreed)
                {   // �������� ���� �ѽ��̸� ����
                    if(UnityEngine.Random.Range(0, 101) < 50)
                    {   // 50% Ȯ���� �ƹ��� �� ����� ������ �ڵ� �ο�
                        CurrGeneraionList.Add(Breed(sortedList[i], sortedList[j]));
                    }
                    else
                    {   // 50% Ȯ���� ��Ӵ� �� ����� ������ �ڵ� �ο�
                        CurrGeneraionList.Add(Breed(sortedList[j], sortedList[i]));
                    }
                    fother.isBreed = true;
                    mother.isBreed = true;
                }
            }
        }

        for(int i = 0; i < sortedList.Count; i++)
        {
            sortedList[i].GetComponent<ChickensBrain>().isBreed = false;
        }


        //while (CurrGeneraionList.Count < beginPopulationSize)
        //{
        //    int bestParentCutoff = sortedList.Count / 4;    // ���� 25% ���Ƹ��� �θ�� ����
        //    for (int i = 0; i < bestParentCutoff - 1; i++)
        //    {
        //        for (int j = 1; j < bestParentCutoff; j++)
        //        {   // ���� ���Ƹ����� DNA�� ���� �����Ͽ� ������ ����
        //            CurrGeneraionList.Add(Breed(sortedList[i], sortedList[j]));
        //            if (CurrGeneraionList.Count == beginPopulationSize) break;

        //            CurrGeneraionList.Add(Breed(sortedList[j], sortedList[i]));
        //            if (CurrGeneraionList.Count == beginPopulationSize) break;
        //        }
        //        if (CurrGeneraionList.Count == beginPopulationSize) break;
        //    }
        //}

        //for (int i = 0; i < sortedList.Count; i++)
        //{
        //    Managers.Resource.Destroy(sortedList[i]);
        //}
        Generation++;
    }

    /// <summary>
    /// �ڽ��� �����ϴ� �Լ�
    /// fother: �ƺ�
    /// mother: ����
    /// </summary>
    private GameObject Breed(GameObject fother, GameObject mother)
    {
        ChickensBrain fotherBrain = fother.GetComponent<ChickensBrain>();
        ChickensBrain motherBrain = mother.GetComponent<ChickensBrain>();

        GameObject eggFlock = GameObject.Find("EggFlock");
        if (eggFlock == null)
        {
            eggFlock = new GameObject { name = "EggFlock" };
        }

        GameObject egg = Spawn("Unit/Egg", mother.transform.localPosition, mother.transform.rotation, eggFlock.transform);
        EggBrain brain = egg.GetComponent<EggBrain>();
        brain.Init();
        brain.MakeDNA();

        if (UnityEngine.Random.Range(0, 100) < 30)
        {   // TODO: �������� ����, 30% Ȯ��
            Debug.Log("�������� �߻�");

            brain.DNA.isMutant = true;

            MeshRenderer render = egg.GetComponent<MeshRenderer>();
            Material[] mats = render.materials;
            mats[0] = eggMaterials[brain.DNA.isMutant];
            render.materials = mats;

            brain.DNA.CombineStat(fotherBrain.DNA, motherBrain.DNA, 2f);
        }
        else
        {   // TODO: �Ϲ� ���Ƹ�
            brain.DNA.CombineStat(fotherBrain.DNA, motherBrain.DNA);
        }
        return egg;

        //GameObject offsetSpring = GetOffset(fother.tag);
        //ChickensBrain brain = offsetSpring.GetComponent<ChickensBrain>();
        //brain.Init();

        //if (UnityEngine.Random.Range(0, 100) < 30)
        //{   // �������� ����, 1% Ȯ��
        //    brain.DNA.isMutant = true;

        //    SkinnedMeshRenderer skin;

        //    switch(offsetSpring.tag)
        //    {
        //        case "Chick":
        //            {
        //                skin = offsetSpring.transform.Find("Toon Chick").GetComponent<SkinnedMeshRenderer>();
        //                Material[] mats = skin.materials;
        //                mats[0] = chickMaterials[brain.DNA.isMutant];
        //                skin.materials = mats;
        //            }
        //            break;

        //        case "Chicken":
        //            {
        //                skin = offsetSpring.transform.Find("Toon Chicken").GetComponent<SkinnedMeshRenderer>();
        //                Material[] mats = skin.materials;
        //                mats[0] = chickenMaterials[brain.DNA.isMutant];
        //                skin.materials = mats;
        //            }
        //            break;
        //    }

        //    Debug.LogWarning($"����Ʈ �߻�! {offsetSpring.name + offsetSpring.transform.GetSiblingIndex()}");
        //}
        //else
        //{
        //    brain.DNA.CombineGenes(fother.GetComponent<ChickensBrain>().DNA, mother.GetComponent<ChickensBrain>().DNA);
        //}
        //return offsetSpring;
    }

    public void Hatch()
    {
        if (eggs.Count == 0) return;        // ���� ���ٸ� ����

        List<GameObject> hatchList = new List<GameObject>();        // ��ȭ �����
        foreach(GameObject egg in eggs)
        {
            EggBrain brain = egg.GetComponent<EggBrain>();
            if(brain.CheckHatchAble())
            {   // ��ȭ �����ϴٸ� ��ȭ ������� �̵�
                hatchList.Add(egg);
            }
            else
            {   // ��ȭ �Ұ����ϴٸ� ��ȭ�� ����
                brain.AddHatchCount();
            }
        }

        int count = hatchList.Count;
        if (count == 0) return;       // ��ȭ ������� ���ٸ� ����

        GameObject chickFlock = GameObject.Find("ChickFlock");
        if (chickFlock == null)
        {
            chickFlock = new GameObject { name = "ChickFlock" };
        }

        for (int i = 0; i < count; i++)
        {
            ChickensBrain newBrain = Spawn("Unit/Chick", hatchList[i].transform.localPosition, hatchList[i].transform.rotation, chickFlock.transform).GetComponent<ChickensBrain>();
            newBrain.MakeDNA();
            newBrain.CopyDNA(hatchList[i].GetComponent<EggBrain>().DNA);
        }

        for (int i = 0; i < count; i++)
        {
            Despawn(hatchList[i]);
        }
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