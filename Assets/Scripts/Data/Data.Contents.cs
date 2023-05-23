using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class Stat
    {
        public int level, maxHp, attack, totalExp;
    }

    [Serializable]
    public class StatData : ILoader<int, Stat>
    {
        public List<Stat> stats = new List<Stat>();

        public Dictionary<int, Stat> MakeDict()
        {
            Dictionary<int, Stat> data = new Dictionary<int, Stat>();
            foreach (Stat stat in stats)
            {
                data.Add(stat.level, stat);
            }
            return data;
        }
    }
}