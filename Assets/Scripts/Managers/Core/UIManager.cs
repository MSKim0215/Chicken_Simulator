 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private Stack<UI_Popup> popupStack = new Stack<UI_Popup>();
    private Dictionary<string, UI_Scene> sceneDict = new Dictionary<string, UI_Scene>();
    private int order = 10;      // 현재 캔버스의 오더

    public T GetScene<T>() where T : UI_Scene
    {
        if (sceneDict.Count <= 0) return null;
        if (sceneDict.ContainsKey(typeof(T).Name)) return sceneDict[typeof(T).Name].GetComponent<T>();
        return null;
    }

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if(root == null)
            {
                root = new GameObject { name = "@UI_Root" };
            }
            return root;
        }
    }

    public void SetCanvas(GameObject _target, bool _sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(_target);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (_sort)
        {   // TODO: 팝업 UI는 order 증가
            canvas.sortingOrder = order;
            order++;
        }
        else
        {   // TODO: 일반 UI는 order 0
            canvas.sortingOrder = 0;
        }
    }

    #region Scene 관리
    /// <summary>
    /// 씬 UI를 호출하는 함수
    /// </summary>
    /// <typeparam name="T">스크립트 이름</typeparam>
    /// <param name="_name">프리팹 이름</param>
    /// <returns></returns>
    public T ShowSceneUI<T>(string _name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(_name)) _name = typeof(T).Name;

        GameObject prefab = Managers.Resource.Instantiate($"UI/Scene/{_name}");
        T sceneUI = Util.GetOrAddComponent<T>(prefab);
        sceneDict.Add(_name, sceneUI);

        prefab.transform.SetParent(Root.transform);

        return sceneUI;
    }

    public void CloseAllSceneUI()
    {
        if (sceneDict.Count <= 0) return;
        sceneDict.Clear();
    }
    #endregion

    #region SubItem 관리

    /// <summary>
    /// 서브아이템 생성 함수
    /// </summary>
    /// <typeparam name="T">스크립트 이름</typeparam>
    /// <param name="_parent">생성될 위치 (부모)</param>
    /// <param name="_name">프리팹 이름</param>
    /// <returns></returns>
    public T MakeSubItem<T>(Transform _parent = null, string _name = null) where T: UI_Base
    {
        if (string.IsNullOrEmpty(_name)) _name = typeof(T).Name;

        GameObject prefab = Managers.Resource.Instantiate($"UI/SubItem/{_name}");
        if(_parent != null)
        {
            prefab.transform.SetParent(_parent);
        }
        return prefab.GetOrAddComponent<T>();
    }
    #endregion

    #region WordSpace 관리
    public T MakeWordSpaceUI<T>(Transform _parent = null, string _name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(_name)) _name = typeof(T).Name;

        GameObject prefab = Managers.Resource.Instantiate($"UI/WorldSpace/{_name}");
        if (_parent != null)
        {
            prefab.transform.SetParent(_parent);
        }

        Canvas canvas = prefab.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        return prefab.GetOrAddComponent<T>();
    }
    #endregion

    #region Popup 관리
    /// <summary>
    /// 팝업 UI를 호출하는 함수
    /// </summary>
    /// <typeparam name="T">스크립트 이름</typeparam>
    /// <param name="_name">프리팹 이름</param>
    /// <returns></returns>
    public T ShowPopupUI<T>(string _name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(_name)) _name = typeof(T).Name;

        GameObject prefab = Managers.Resource.Instantiate($"UI/Popup/{_name}");
        T popup = Util.GetOrAddComponent<T>(prefab);
        popupStack.Push(popup);

        prefab.transform.SetParent(Root.transform);

        return popup;
    }

    /// <summary>
    /// 닫는 팝업 UI가 맞는지 비교 후 닫는 함수
    /// </summary>
    /// <param name="_popup">닫히는 팝업</param>
    public void ClosePopupUI(UI_Popup _popup)
    {
        if (popupStack.Count == 0) return;

        if(popupStack.Peek() != _popup)
        {
            Debug.LogWarning("Close Popup Failed!");
            return;
        }
        ClosePopupUI();
    }

    /// <summary>
    /// 팝업 UI를 닫는 함수
    /// </summary>
    public void ClosePopupUI()
    {
        if (popupStack.Count == 0) return;

        UI_Popup popup = popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);

        order--;
    }

    /// <summary>
    /// 모든 팝업 UI를 닫는 함수
    /// </summary>
    public void CloseAllPopupUI()
    {
        while(popupStack.Count > 0)
        {
            ClosePopupUI();
        }
    }
    #endregion

    public void Clear()
    {
        CloseAllSceneUI();
        CloseAllPopupUI();
    }
}