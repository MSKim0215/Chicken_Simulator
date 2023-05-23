using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class UI_Base : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> objDict = new Dictionary<Type, UnityEngine.Object[]>();

    public abstract void Init();

    private void Start()
    {
        Init();
    }

    /// <summary>
    /// UI 오브젝트를 찾아서 배열에 저장하는 함수
    /// </summary>
    /// <typeparam name="T">UI 오브젝트 타입</typeparam>
    /// <param name="_type">enum 타입</param>
    protected void Bind<T>(Type _type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(_type);
        UnityEngine.Object[] objs = new UnityEngine.Object[names.Length];
        objDict.Add(typeof(T), objs);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
            {   // 게임오브젝트 타입일 경우
                objs[i] = Util.FindChild(gameObject, names[i], true);
            }
            else
            {   // 그 외의 타입일 경우
                objs[i] = Util.FindChild<T>(gameObject, names[i], true);
            }

            if (objs[i] == null)
            {
                Debug.LogWarning($"Failed to Bind. {names[i]}");
            }
        }
    }

    /// <summary>
    /// UI 오브젝트를 꺼내서 쓰는 함수
    /// </summary>
    /// <typeparam name="T">UI 오브젝트 타입</typeparam>
    /// <param name="_index">UI 오브젝트의 인덱스</param>
    /// <returns></returns>
    protected T Get<T>(int _index) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (!objDict.TryGetValue(typeof(T), out objects)) return null;
        return objects[_index] as T;
    }

    /// <summary>
    /// 이벤트 타입에 따라 UI 오브젝트에 이벤트를 부여
    /// </summary>
    /// <param name="_target">목표 오브젝트</param>
    /// <param name="_action">이벤트</param>
    /// <param name="_type">이벤트 타입</param>
    public static void BindEvent(GameObject _target, Action<PointerEventData> _action, Define.UIEvent _type = Define.UIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(_target);

        switch (_type)
        {
            case Define.UIEvent.Click:
                {
                    evt.OnClickHandler -= _action;
                    evt.OnClickHandler += _action;
                }
                break;
        }
    }

    protected GameObject GetObject(int _index) => Get<GameObject>(_index);
    protected Text GetText(int _index) => Get<Text>(_index);
    protected Image GetImage(int _index) => Get<Image>(_index);
    protected Button GetButton(int _index) => Get<Button>(_index);
}