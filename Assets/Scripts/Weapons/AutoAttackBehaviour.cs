using System.Collections.Generic;
using System.Linq;
using Common.Pools;
using Common.Runtime;
using Common.Stats;
using Common.Utils;
using Enemies;
using UnityEngine;

namespace Weapons
{
    [RequireComponent(typeof(IRuntimeEntity))]
    public abstract class AutoAttackBehaviour : MonoBehaviour, ISpwanPoolable
    {
        [Header("Attack Behaviour")]
        [SerializeField] private float targetDetectionPeriod = 0.5f;
        [SerializeField] private TriggerAccessor enemyTrigger;
        [SerializeField] private CircleCollider2D triggerCollider;
        private readonly HashSet<RuntimeEnemy> _targets = new();
        
        protected float LastTargetDetectionTime;
        protected float LastShotTime;
        
        protected IRuntimeEntity SelfEntity;
        protected RuntimeEnemy AcquiredTarget;
        protected RuntimeStat RadiusStat;
        protected RuntimeStat SpeedStat;
        protected RuntimeStat DamageStat;
        
        public virtual void Spawn()
        {
            LastShotTime = 0;
            LastTargetDetectionTime = 0;
            SelfEntity = GetComponent<IRuntimeEntity>();
            
            if (!SelfEntity.Stats.TryGet(x => x.Name == "Radius", out RadiusStat))
                RadiusStat = SelfEntity.Stats.AddStat(new StatData{Name = "Radius", Value = 2f});
            
            if (!SelfEntity.Stats.TryGet(x => x.Name == "AttackSpeed", out SpeedStat))
                SpeedStat = SelfEntity.Stats.AddStat(new StatData{Name = "AttackSpeed", Value = 2f});
            
            if (!SelfEntity.Stats.TryGet(x => x.Name == "Damage", out DamageStat))
                DamageStat = SelfEntity.Stats.AddStat(new StatData{Name = "Damage", Value = 1f});

            triggerCollider.radius = RadiusStat.TotalValue;
            enemyTrigger.AddOnEnterCallback(c =>
            {
                if (c.TryGetComponent(out RuntimeEnemy enemy) && enemy.IsAlive)
                    _targets.Add(enemy);
            });
            
            enemyTrigger.AddOnExitCallback(c =>
            {
                if (c.TryGetComponent(out RuntimeEnemy enemy))
                {
                    _targets.Remove(enemy);
                    
                    if (enemy == AcquiredTarget)
                        AcquiredTarget = null;
                }
            });
        }

        public virtual void Release()
        {
            enemyTrigger.RemoveAllCallbacks(this);
            _targets.Clear();
            AcquiredTarget = null;
            RadiusStat = null;
            SpeedStat = null;
            DamageStat = null;
        }

        protected virtual void Update()
        {
            if (!TryFindTarget())
                return;

            if (PrepareToAttack())
                Attack();
        }

        protected virtual bool TryFindTarget()
        {
            if (AcquiredTarget is { IsAlive: true } && triggerCollider.OverlapPoint(AcquiredTarget.transform.position)) 
                return true;

            if (LastTargetDetectionTime > Time.time) 
                return false;
            
            AcquiredTarget = _targets.FirstOrDefault(x => x.IsAlive);
            LastTargetDetectionTime = Time.time + targetDetectionPeriod;
            return AcquiredTarget is {IsAlive: true};
        }

        protected virtual bool PrepareToAttack()
        {
            if (LastShotTime < Time.time)
            {
                LastShotTime = Time.time + (1f / SpeedStat.TotalValue);
                return true;
            }

            return false;
        }

        protected abstract void Attack();
        
        protected virtual void ApplyDamage(RuntimeEnemy enemy)
        {
            if (enemy is not {IsAlive:true} || !enemy.TryGetComponent(out IDamagable damagable))
                return;
            
            damagable.ApplyDamage(new DamageContext
            {
                Source = SelfEntity,
                Target = AcquiredTarget,
                Amount = DamageStat.TotalValue
            });
        }
    }
}