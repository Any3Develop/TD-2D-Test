using System.Collections.Generic;
using System.Linq;
using Common.Stats;
using Common.Utils;
using Enemies;
using Unity.Mathematics;
using UnityEngine;
using VFX;
using Zenject;

namespace Weapons
{
    public class ProjectileAOE : Projectile
    {
        [SerializeField] private TriggerAccessor enemyTrigger;
        [SerializeField] private CircleCollider2D aoeCollider;
        [SerializeField] private VfxView vfxPrefab;
        private readonly HashSet<RuntimeEnemy> _hitTargets = new();
        [Inject] private readonly IInstantiator _instantiator;
        public float AOERadius = 0.5f;
        
        public override void Spawn()
        {
            base.Spawn();
            aoeCollider.radius = AOERadius;
            enemyTrigger.AddOnEnterCallback(c =>
            {
                if (c.TryGetComponent(out RuntimeEnemy target) && target.IsAlive)
                    _hitTargets.Add(target);
            });
            enemyTrigger.AddOnExitCallback(c =>
            {
                if (c.TryGetComponent(out RuntimeEnemy target))
                    _hitTargets.Remove(target);
            });
        }

        protected override void Hit()
        {
            if (vfxPrefab)
            {
                // TODO pool!
                var vfx = _instantiator.InstantiatePrefabForComponent<VfxView>(vfxPrefab);
                vfx.transform.position = transform.position;
                vfx.Stats.AddStat(new StatData { Name = "Radius", Value = AOERadius });
            }
            
            base.Hit();
        }

        public override void Release()
        {
            base.Release();
            enemyTrigger.RemoveAllCallbacks(this);
            _hitTargets.Clear();
        }

        public override RuntimeEnemy[] GetHitTargets() 
            => _hitTargets.ToArray();
    }
}