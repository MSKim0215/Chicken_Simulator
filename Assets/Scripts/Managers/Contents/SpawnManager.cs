using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("현재 생성된 먹이 개수")]
    [SerializeField] private int feedCount = 0;

    [Header("현재 생성 대기중인 먹이 개수")]
    [SerializeField] private int reserveCount = 0;

    [Header("유지 해야하는 먹이 개수")]
    [SerializeField] private int keepFeedCount = 0;

    private float spawnTime;        // 스폰 시간

    private void Start()
    {
        Managers.Game.OnSpawnEvent -= AddFeedCount;
        Managers.Game.OnSpawnEvent += AddFeedCount;

        spawnTime = 2f;

        for (int i = 0; i < (int)Define.FeedMakeCount.AM; i++)
        {
            Managers.Game.Spawn(Define.WorldObject.Feed, "Unit/Feed", transform);
        }
    }

    private void Update()
    {
        while(reserveCount + feedCount < keepFeedCount)
        {
            StartCoroutine(ReserveSpawn());
        }
    }

    private IEnumerator ReserveSpawn()
    {
        reserveCount++;

        yield return new WaitForSeconds(
            (Managers.Timer.Meridiem == TimerManager.TimeMeridiem.AM) ?
            spawnTime:
            spawnTime / 2f      // 밤에는 먹이 생성 주기 절반 감소
            );

        Managers.Game.Spawn(Define.WorldObject.Feed, "Unit/Feed", transform);
        reserveCount--;
    }

    public void AddFeedCount(int value) => feedCount += value;

    public void SetKeepFeedCount(int value) => keepFeedCount = value;
}