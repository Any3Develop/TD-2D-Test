using System;
using Common.Pools;
using Common.Runtime;
using Common.Stats;
using DG.Tweening;
using Gameplay.Waves;
using UnityEngine;
using Zenject;
using IPoolable = Common.Pools.IPoolable;

namespace Enemies
{
    public class RuntimeEnemy : MonoBehaviour, IPoolable, IRuntimeEntity
    {
        [SerializeField] private Collider2D mainCollider;
        
        public string Id { get; } = Guid.NewGuid().ToString();
        public GameObject Container { get; private set; }
        [Inject] public StatsCollection Stats { get; private set; }
        [Inject] public IPool<RuntimeEnemy> Pool { get; private set; }
        public EnemyConfig Config { get; private set; }
        public bool IsAlive { get; private set; }
        
        [Inject] private readonly WaveSpawner _waveSpawner;
        private RuntimeStat _healthStat;
        private EnemyAnimator _animator;
        
        public virtual void Init(EnemyConfig config)
        {
            Config = config;
            Container = gameObject;
            _animator = GetComponent<EnemyAnimator>();
     
            config.Stats.ForEach(data => Stats.AddStat(data));
            
            if (!Stats.TryGet(x => x.Name == "Health", out _healthStat))
                _healthStat = Stats.AddStat(new StatData{Name = "Health", Value = 100, Previous = 100});
            
            _healthStat.OnChaged += OnHealthsChanged;
            IsAlive = _healthStat.TotalValue > 0;
            Container.SetActive(true);
            if (mainCollider)
                mainCollider.enabled = true;
        }

        private void OnHealthsChanged(RuntimeStat stat)
        {
            IsAlive = stat.TotalValue > 0;
            if (stat.TotalValue > 0)
                return;
            
            if (mainCollider)
                mainCollider.enabled = false;
            
            stat.OnChaged -= OnHealthsChanged;
            _waveSpawner.OnDied(this);
            DOVirtual.DelayedCall(_animator?.dyingTime ?? 0f, SelfRelease);
        }

        public virtual void Release()
        {
            if (_healthStat != null)
                _healthStat.OnChaged -= OnHealthsChanged;
            
            
            if (mainCollider)
                mainCollider.enabled = false;
            
            Stats.Clear();
            Container.SetActive(false);
            Config = null;
        }

        public virtual void OnDestroy()
        {
            Pool?.Release(Config?.Prototype.name, this);
            Stats = null;
            Config = null;
        }

        public void SelfRelease()
        {
            if (!Config)
                return;
            
            Pool.Release(Config.Prototype.name, this);
        }
    }
}