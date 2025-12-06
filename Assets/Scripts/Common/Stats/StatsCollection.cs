using System;
using System.Linq;
using Common.Repositiory;
using UnityEngine.Pool;

namespace Common.Stats
{
    public class StatsCollection : Repository<RuntimeStat>
    {
        private static readonly ObjectPool<RuntimeStat> Pool = new(() => new RuntimeStat());

        public RuntimeStat AddStat(StatData data)
        {
            var stat = Pool.Get();
            stat.Init(Guid.NewGuid().ToString(), data.Name, data.Value, data.Previous);
            Add(stat);
            return stat;
        }

        public new int RemoveAll(Predicate<RuntimeStat> predicate)
        {
            return FindAll(predicate).ToArray().Count(Remove);
        }

        public new bool Remove(RuntimeStat stat)
        {
            if (!base.Remove(stat))
                return false;

            Pool.Release(stat);
            return true;
        }

        public new void Clear()
        {
            foreach (var stat in this)
                Pool.Release(stat);

            base.Clear();
        }
        
        public bool TryGet(Predicate<RuntimeStat> predicate, out RuntimeStat stat)
        {
            stat = Find(predicate);
            return stat != null;
        }
    }
}