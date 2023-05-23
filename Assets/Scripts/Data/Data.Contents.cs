using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class ChickenStat
    {
        public int Level, ExpMax, HpMax, AttackPower, Defense;
        public float MoveSpeed, EatRange;
    }

    [Serializable]
    public class ChickenStatData : ILoader<int, ChickenStat>
    {
        public List<ChickenStat> ChickenStats = new List<ChickenStat>();

        public Dictionary<int, ChickenStat> MakeDict()
        {
            Dictionary<int, ChickenStat> data = new Dictionary<int, ChickenStat>();
            foreach(ChickenStat stat in ChickenStats)
            {
                data.Add(stat.Level, stat);
            }
            return data;
        }
    }

    [Serializable]
    public class ChickStatData : ILoader<int, ChickenStat>
    {
        public List<ChickenStat> ChickStats = new List<ChickenStat>();

        public Dictionary<int, ChickenStat> MakeDict()
        {
            Dictionary<int, ChickenStat> data = new Dictionary<int, ChickenStat>();
            foreach (ChickenStat stat in ChickStats)
            {
                data.Add(stat.Level, stat);
            }
            return data;
        }
    }

    [Serializable]
    public class EggStatData : ILoader<int, ChickenStat>
    {
        public List<ChickenStat> EggStats = new List<ChickenStat>();

        public Dictionary<int, ChickenStat> MakeDict()
        {
            Dictionary<int, ChickenStat> data = new Dictionary<int, ChickenStat>();
            foreach (ChickenStat stat in EggStats)
            {
                data.Add(stat.Level, stat);
            }
            return data;
        }
    }
}