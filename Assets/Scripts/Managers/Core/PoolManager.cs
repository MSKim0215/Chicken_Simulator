using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    private class Pool
    {
        public GameObject Original { private set; get; }
        public Transform Root { set; get; }

        Queue<Poolable> poolQueue = new Queue<Poolable>();

        /// <summary>
        /// 풀 초기화 함수
        /// </summary>
        /// <param name="_original">원본 오브젝트</param>
        /// <param name="_count">풀 생성시 초기 생성 개수</param>
        public void Init(GameObject _original, int _count = 5)
        {
            Original = _original;
            Root = new GameObject().transform;
            Root.name = $"{Original.name}_Root";

            for (int i = 0; i < _count; i++)
            {
                Push(Create());
            }
        }

        /// <summary>
        /// 풀 오브젝트 생성함수
        /// </summary>
        /// <returns></returns>
        private Poolable Create()
        {
            GameObject prefab = Object.Instantiate<GameObject>(Original);
            prefab.name = Original.name;
            return prefab.GetOrAddComponent<Poolable>();
        }

        /// <summary>
        /// 풀 오브젝트 초기화 함수
        /// </summary>
        /// <param name="_able">풀 오브젝트</param>
        public void Push(Poolable _able)
        {
            if (_able == null) return;

            _able.transform.SetParent(Root);
            _able.gameObject.SetActive(false);
            _able.isUsing = false;

            poolQueue.Enqueue(_able);
        }

        /// <summary>
        /// 풀 오브젝트 사용하는 함수
        /// </summary>
        /// <param name="_parent">부모</param>
        /// <returns></returns>
        public Poolable Pop(Transform _parent)
        {
            Poolable able;

            if(poolQueue.Count > 0)
            {
                able = poolQueue.Dequeue();
            }
            else
            {
                able = Create();
            }

            able.gameObject.SetActive(true);

            if(_parent == null)
            {   // TODO: DontDestroyOnLoad 해제
                able.transform.parent = Managers.Scene.CurrentScene.transform;
            }

            able.transform.SetParent(_parent);
            able.isUsing = true;
            return able;
        }
    }

    private Dictionary<string, Pool> pools = new Dictionary<string, Pool>();
    private Transform root;

    public void Init()
    {
        if(root == null)
        {
            root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(root);
        }
    }

    /// <summary>
    /// 풀에 집어넣는 함수
    /// </summary>
    /// <param name="_able">해당 객체</param>
    public void Push(Poolable _able)
    {
        string name = _able.name;
        if (!pools.ContainsKey(name)) GameObject.Destroy(_able.gameObject);
        pools[name].Push(_able);
    }

    /// <summary>
    /// 풀에서 꺼내서 사용하는 함수
    /// </summary>
    /// <param name="_original">원본 오브젝트</param>
    /// <param name="_parent">해당 오브젝트의 부모</param>
    public Poolable Pop(GameObject _original, Transform _parent = null)
    {
        if(!pools.ContainsKey(_original.name)) CreatePool(_original);
        return pools[_original.name].Pop(_parent);
    }

    public void CreatePool(GameObject _original, int _count = 5)
    {
        Pool pool = new Pool();
        pool.Init(_original, _count);
        pool.Root.SetParent(root);
        pools.Add(_original.name, pool);
    }

    /// <summary>
    /// 원본 오브젝트 리턴 함수
    /// </summary>
    /// <param name="_name">오브젝트 이름</param>
    /// <returns></returns>
    public GameObject GetOriginal(string _name)
    {
        if (!pools.ContainsKey(_name)) return null;
        return pools[_name].Original;
    }

    public void Clear()
    {
        foreach(Transform child in root)
        {
            GameObject.Destroy(child.gameObject);
        }
        pools.Clear();
    }
}