using Common.Pools;
using UnityEngine;
using Weapons;
using Zenject;

namespace Buildings
{
    public class BuildingTowerAttackBehaviour : AutoAttackBehaviour
    {
        [Header("Projectile Behaviour")]
        [SerializeField] protected bool followTarget;
        [SerializeField] protected TowerRotator towerRotator;
        [SerializeField] protected Transform aimTransform;
        [SerializeField] protected Projectile projectile;

        [Inject] protected readonly IInstantiator Instantiator;
        [Inject] protected readonly IPool<Projectile> Pool;
        private static Transform _projectilesContainer;
        
        public override void Spawn()
        {
            if (!Pool.IsRegistered(projectile.name))
                Pool.Register(projectile.name, () => Instantiator.InstantiatePrefabForComponent<Projectile>(projectile, _projectilesContainer));
            
            if (!_projectilesContainer)
                _projectilesContainer = new GameObject("Projectiles").transform;
            
            base.Spawn();
        }

        protected override bool PrepareToAttack()
        {
            return (!towerRotator || towerRotator.RotateToTarget(AcquiredTarget.transform.position)) && base.PrepareToAttack();
        }

        protected override void Attack()
        {
            var instance = Pool.Get(projectile.name);
            
            if (followTarget)
                instance.SteeringTarget = AcquiredTarget.transform;
            else 
                instance.Destination = AcquiredTarget.transform.position;
                
            instance.Self.position = aimTransform.position;
            instance.OnHitCallback = () =>
            {
                if (instance.GetHitTargets() is {} targets)
                {
                    foreach (var target in targets)
                        ApplyDamage(target);
                }
                else
                {
                    ApplyDamage(AcquiredTarget);
                }
                    
                Pool.Release(projectile.name, instance);
            };
                
            Pool.Occupy(projectile.name, instance);
            return;
        }
    }
}