using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;

        //gameObject.GetOrAddComponent<CursorController>();

        //GameObject player = Managers.Game.Spawn(Define.WorldObject.Player, "Player");
        //Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);

        //GameObject spawningPool = new GameObject { name = "SpawningPool" };
        //SpawningPool pool = spawningPool.GetOrAddComponent<SpawningPool>();
        //pool.SetKeepMonsterCount(5);

        // ���� UI ����
        //Managers.UI.ShowSceneUI<UI_UnitFrame>();
        //Managers.UI.ShowSceneUI<UI_ExpBar>();
        //Managers.UI.ShowSceneUI<UI_SkillBoxes>();
        //Managers.UI.ShowSceneUI<UI_Joystick>();
        //Managers.UI.ShowSceneUI<UI_AttackButton>().SetPlayer(player);

        for(int i = 0; i < Managers.Game.ChickensGroupFlock.startingCount; i++)
        {
            Managers.Game.Spawn(Define.WorldObject.ChickGroup, "Unit/Chick");
        }



        //for(int i = 0; i < Managers.Game.BeginPopulationSize; i++)
        //{
        //    Managers.Game.Spawn("Unit/Chick", chickFlock.transform).GetComponent<ChickensBrain>().MakeDNA();
        //}

        //GameObject spawner = GameObject.Find("FeedSpawner");
        //if (spawner == null)
        //{
        //    spawner = new GameObject { name = "FeedSpawner" };
        //}
        //SpawnManager feedSpawner = spawner.GetOrAddComponent<SpawnManager>();
    }

    public override void Clear()
    {
        
    }
}