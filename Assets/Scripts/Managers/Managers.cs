using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance;

    public static Managers Instance
    {
        get
        {
            Init();
            return s_instance;
        }
    }

    #region Core
    private ResourceManager resource = new ResourceManager();
    private UIManager ui = new UIManager();
    private SceneManagerEx scene = new SceneManagerEx();
    private SoundManager sound = new SoundManager();
    private PoolManager pool = new PoolManager();
    private DataManager data = new DataManager();

    public static ResourceManager Resource => Instance.resource;
    public static UIManager UI => Instance.ui;
    public static SceneManagerEx Scene => Instance.scene;
    public static SoundManager Sound => Instance.sound;
    public static PoolManager Pool => Instance.pool;
    public static DataManager Data => Instance.data;
    #endregion

    #region Contents
    private GameManager game = new GameManager();

    public static GameManager Game => Instance.game;
    #endregion

    private void Start()
    {
        Init();
    }

    private static void Init()
    {
        if(s_instance == null)
        {
            GameObject target = GameObject.Find("@Managers");
            if(target == null)
            {
                target = new GameObject { name = "@Managers" };
                target.AddComponent<Managers>();
            }

            DontDestroyOnLoad(target);
            s_instance = target.GetComponent<Managers>();

            s_instance.game.Init();
            s_instance.data.Init();
            s_instance.pool.Init();
            s_instance.sound.Init();
        }
    }

    public static void Clear()
    {
        Sound.Clear();
        Scene.Clear();
        UI.Clear();
        Pool.Clear();
    }

    private void Update()
    {
        Game.OnUpdate();
    }

    GUIStyle guiStyle = new GUIStyle();
    private void OnGUI()
    {
        guiStyle.fontSize = 25;
        guiStyle.normal.textColor = Color.white;
        GUI.BeginGroup(new Rect(10, 10, 250, 250));
        GUI.Box(new Rect(0, 0, 140, 140), "목록", guiStyle);
        GUI.Label(new Rect(10, 25, 200, 30), "현재 세대: " + game.Generation, guiStyle);
        GUI.Label(new Rect(10, 50, 200, 30), string.Format("세대 교체 시간: {0:0.00}", GameManager.elapsed), guiStyle);
        GUI.Label(new Rect(10, 75, 200, 30), "인구수: " + game.CurrGeneraionList.Count, guiStyle);
        GUI.EndGroup();
    }
}
