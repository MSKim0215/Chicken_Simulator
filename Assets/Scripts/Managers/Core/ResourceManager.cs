using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string _path) where T : Object
    {
        if(typeof(T) == typeof(GameObject))
        {   // TODO: 이미 원본 오브젝트를 들고 있다면 바로 사용
            string name = _path;
            int index = name.LastIndexOf('\\');
            if(index >= 0) name = name.Substring(index + 1);

            GameObject original = Managers.Pool.GetOriginal(name);
            if (original != null) return original as T;
        }

        return Resources.Load<T>(_path);
    }

    /// <summary>
    /// 프리팹 생성 함수
    /// </summary>
    /// <param name="_path">프리팹 경로 {Default: Prefabs/}</param>
    /// <param name="_parent">프리팹 부모</param>
    /// <returns></returns>
    public GameObject Instantiate(string _path, Transform _parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{_path}");
        if(prefab == null)
        {
            Debug.LogWarning($"프리팹을 찾을 수 없습니다. {_path}");
            return null;
        }

        if(prefab.GetComponent<Poolable>() != null)
        {   // TODO: 풀링 안에 있는 오브젝트인지 체크
            return Managers.Pool.Pop(prefab, _parent).gameObject;
        }

        GameObject result = Object.Instantiate(prefab, _parent);
        result.name = prefab.name;
        return result;
    }

    public GameObject Instantiate(string _path, Vector3 _dir, Quaternion _eulr, Transform _parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{_path}");
        if (prefab == null)
        {
            Debug.LogWarning($"프리팹을 찾을 수 없습니다. {_path}");
            return null;
        }

        if (prefab.GetComponent<Poolable>() != null)
        {   // TODO: 풀링 안에 있는 오브젝트인지 체크
            return Managers.Pool.Pop(prefab, _parent).gameObject;
        }

        GameObject result = Object.Instantiate(prefab, _dir, _eulr, _parent);
        result.name = prefab.name;
        return result;
    }

    public void Destroy(GameObject _target)
    {
        if (_target == null) return;

        Poolable able = _target.GetComponent<Poolable>();
        if(able != null)
        {   // TODO: 풀링에서 사용되는 오브젝트인지 체크
            Managers.Pool.Push(able);
            return;
        }
        Object.Destroy(_target);
    }
}