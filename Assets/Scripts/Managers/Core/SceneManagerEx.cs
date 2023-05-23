using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene => GameObject.FindObjectOfType<BaseScene>();

    /// <summary>
    /// 씬의 타입에 따라 해당 씬을 로딩하는 함수
    /// </summary>
    /// <param name="_type">씬 타입</param>
    public void LoadScene(Define.Scene _type)
    {
        Managers.Clear();
        SceneManager.LoadScene(GetSceneName(_type));
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }

    private string GetSceneName(Define.Scene _type) => System.Enum.GetName(typeof(Define.Scene), _type);
}