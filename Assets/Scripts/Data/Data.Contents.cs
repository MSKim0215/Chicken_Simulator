using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class BaseChickenLevelTable
    {
        public int Level, ExpMax, ChickGrowMax;
    }

    [Serializable]
    public class BaseChickenStat
    {
        public int Index, HpMax, EatPower, Defense;
        public float MoveSpeed, EatRange;
    }

    [Serializable]
    public class ChickenLevelTableData : ILoader<int, BaseChickenLevelTable>
    {
        public List<BaseChickenLevelTable> BaseChickenLevelTable = new List<BaseChickenLevelTable>();

        public Dictionary<int, BaseChickenLevelTable> MakeDict()
        {
            Dictionary<int, BaseChickenLevelTable> data = new Dictionary<int, BaseChickenLevelTable>();
            foreach (BaseChickenLevelTable table in BaseChickenLevelTable)
            {
                data.Add(table.Level, table);
            }
            return data;
        }
    }

    [Serializable]
    public class ChickenStatData : ILoader<int, BaseChickenStat>
    {
        public List<BaseChickenStat> BaseChickenStats = new List<BaseChickenStat>();

        public Dictionary<int, BaseChickenStat> MakeDict()
        {
            Dictionary<int, BaseChickenStat> data = new Dictionary<int, BaseChickenStat>();
            foreach (BaseChickenStat stat in BaseChickenStats)
            {
                data.Add(stat.Index, stat);
            }
            return data;
        }
    }
}