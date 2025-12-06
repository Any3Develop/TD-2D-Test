using Common.Pools;
using Common.Runtime;
using Common.Stats;
using Loots;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(IRuntimeEntity))]
    public class EnemyLootDrop : LootDrop, ISpwanPoolable
    {
        private IRuntimeEntity _selfEntity;
        private RuntimeStat _healths;
        
        public void Spawn()
        {
            _selfEntity = GetComponent<IRuntimeEntity>();
            if (_selfEntity.Stats.TryGet(x => x.Name == "Health", out _healths))
                _healths.OnChaged += OnHealthsChanged;
        }

        private void OnHealthsChanged(RuntimeStat stat)
        {
            if (stat.TotalValue > 0)
                return;
            
            _healths.OnChaged -= OnHealthsChanged;
            Drop();
        }

        public void Release()
        {
            _selfEntity = null;
            if (_healths != null)
                _healths.OnChaged -= OnHealthsChanged;
        }

        private void OnDestroy()
        {
            if (_healths != null)
                _healths.OnChaged -= OnHealthsChanged;
            
            _healths = null;
            _selfEntity = null;
        }
    }
}