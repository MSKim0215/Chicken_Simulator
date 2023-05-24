using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("���� ������ ���� ����")]
    [SerializeField] private int feedCount = 0;

    [Header("���� ���� ������� ���� ����")]
    [SerializeField] private int reserveCount = 0;

    [Header("���� �ؾ��ϴ� ���� ����")]
    [SerializeField] private int keepFeedCount = 0;

    private float spawnTime;        // ���� �ð�

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
            spawnTime / 2f      // �㿡�� ���� ���� �ֱ� ���� ����
            );

        Managers.Game.Spawn(Define.WorldObject.Feed, "Unit/Feed", transform);
        reserveCount--;
    }

    public void AddFeedCount(int value) => feedCount += value;

    public void SetKeepFeedCount(int value) => keepFeedCount = value;
}