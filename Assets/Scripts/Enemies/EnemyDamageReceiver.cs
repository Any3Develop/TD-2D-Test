using Common.Pools;
using Common.Runtime;
using Common.Stats;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(IRuntimeEntity))]
    public class EnemyDamageReceiver : MonoBehaviour, IDamagable, ISpwanPoolable
    {
        private IRuntimeEntity _selfEntity;
        private EnemyAnimator _animator;
        private RuntimeStat _stat;
        private bool _initialized;

        private void Awake()
        {
            _selfEntity = GetComponent<IRuntimeEntity>();
            _animator = GetComponent<EnemyAnimator>();
        }

        public void Spawn()
        {
            if (!_selfEntity.Stats.TryGet(x => x.Name == "Health", out _stat))
                _selfEntity.Stats.AddStat(new StatData{Name = "Health", Value = 10, Previous = 10});
            
            _initialized = true;
        }

        public void Release()
        {
            _initialized = false;
            _stat = null;
        }
        
        public void ApplyDamage(DamageContext ctx)
        {
            if (!_initialized)
                return;
            
            _stat.SetValue(_stat.Value - ctx.Amount);
            if (!_animator)
                return;
            
            _animator.HitReaction();
            
            if (_stat.TotalValue <= 0)
            {
                _animator.Die();
                _initialized = false;
            }
        }
    }
}