using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour
{
    public static T GetOrAddComponent<T>(GameObject _target) where T : UnityEngine.Component
    {
        T component = _target.GetComponent<T>();
        if (component == null)
        {
            component = _target.AddComponent<T>();
        }
        return component;
    }

    /// <summary>
    /// 자식 오브젝트 (게임 오브젝트 타입용) 탐색 함수
    /// </summary>
    /// <param name="_target">목표 오브젝트</param>
    /// <param name="_name">목표 오브젝트 이름 -> 이름을 받지 않으면 이름 비교안함</param>
    /// <param name="_recursive">자식만 찾을 것인지 or 최하위 자식까지 찾을 것인지</param>
    /// <returns></returns>
    public static GameObject FindChild(GameObject _target, string _name = null, bool _recursive = false)
    {
        Transform transform = FindChild<Transform>(_target, _name, _recursive);
        if (transform != null) return transform.gameObject;
        return null;
    }

    /// <summary>
    /// 자식 오브젝트 탐색 함수
    /// </summary>
    /// <typeparam name="T">목표 오브젝트 타입</typeparam>
    /// <param name="_target">목표 오브젝트</param>
    /// <param name="_name">목표 오브젝트 이름 -> 이름을 받지 않으면 이름 비교안함</param>
    /// <param name="_recursive">자식만 찾을 것인지 or 최하위 자식까지 찾을 것인지</param>
    /// <returns></returns>
    public static T FindChild<T>(GameObject _target, string _name = null, bool _recursive = false) where T: UnityEngine.Object
    {
        if (_target == null) return null;

        if(!_recursive)
        {   // TODO: 자신의 직속 자식만 찾는 경우
            for(int i = 0; i < _target.transform.childCount; i++)
            {
                Transform transform = _target.transform.GetChild(i);
                if(string.IsNullOrEmpty(_name) || transform.name == _name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null) return component;
                }
            }
        }
        else
        {   // TODO: 자신의 최하위 자식까지 찾는 경우
            foreach(T component in _target.GetComponentsInChildren<T>())
            {
                if(string.IsNullOrEmpty(_name) || component.name == _name)
                {   
                    return component;
                }
            }
        }
        return null;
    }
}