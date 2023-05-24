using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("���� ������ ���� ����")]
    [SerializeField] private int feedCount = 0;

    [Header("���� �ؾ��ϴ� ���� ����")]
    [SerializeField] private int keepFeedCount = 0;

    private float spawnTime;        // ���� �ð�
    private int reserveCount;       // ���� ����� ���� ����

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