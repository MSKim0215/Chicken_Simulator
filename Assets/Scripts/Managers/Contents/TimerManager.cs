using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager
{
    public enum TimeMeridiem
    {
        AM, PM
    }

    private TimeMeridiem meridiem;    
    private float changeTimes;         // 변경 기준 시간

    public float CurrentTimes { private set; get; } = 0f;        // 현재 시간

    public TimeMeridiem Meridiem
    {
        get => meridiem;
        set
        {
            meridiem = value;

            GameObject spawner = GameObject.Find("FeedSpawner");
            if(spawner == null)
            {
                spawner = new GameObject { name = "FeedSpawner" };
            }
            SpawnManager feedSpawner = spawner.GetOrAddComponent<SpawnManager>();
            if (feedSpawner == null) return;

            if(Meridiem == TimeMeridiem.AM)
            {
                feedSpawner.SetKeepFeedCount((int)Define.FeedMakeCount.AM);
            }
            else
            {
                feedSpawner.SetKeepFeedCount((int)Define.FeedMakeCount.PM);
            }
        }
    }

    public void Init()
    {
        Meridiem = TimeMeridiem.AM;
        changeTimes = 5f;
    }

    public void OnUpdate()
    {
        if(CurrentTimes < changeTimes)
        {
            CurrentTimes += Time.deltaTime;
            if(CurrentTimes > changeTimes)
            {
                if (Meridiem == TimeMeridiem.AM) Meridiem = TimeMeridiem.PM;
                else
                {
                    Meridiem = TimeMeridiem.AM;
                    Managers.Game.Hatch();
                    Managers.Game.BreedNewPopulation();
                    Managers.Game.Evolution();
                }

                CurrentTimes = 0f;
            }
        }
    }
}