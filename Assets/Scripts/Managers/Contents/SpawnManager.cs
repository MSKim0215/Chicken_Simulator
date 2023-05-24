using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("현재 생성된 먹이 개수")]
    [SerializeField] private int feedCount = 0;

    [Header("유지 해야하는 먹이 개수")]
    [SerializeField] private int keepFeedCount = 0;

    private float spawnTime;        // 스폰 시간
    private int reserveCount;       // 생성 예약된 먹이 개수

    private void Start()
    {
        Managers.Game.OnSpawnEvent -= AddFeedCount;
        Managers.Game.OnSpawnEvent += AddFeedCount;

        spawnTime = 2f;
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

        yield return new WaitForSeconds(spawnTime);

        Managers.Game.Spawn(Define.WorldObject.Feed, "Unit/Feed");
        reserveCount--;
    }

    public void AddFeedCount(int value) => feedCount += value;
    public void SetKeepFeedCount(int value) => keepFeedCount = value;
}