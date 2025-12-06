using System;
using Common.Pools;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Enemies
{
    public class EnemyFactory : IDisposable
    {
        private readonly Transform _sceneContainer;
        private readonly IPool<RuntimeEnemy> _pool;
        private readonly IInstantiator _instantiator;

        public EnemyFactory(
            IPool<RuntimeEnemy> pool,
            IInstantiator instantiator)
        {
            this._pool = pool;
            this._instantiator = instantiator;
            _sceneContainer = new GameObject("Enemies").transform;
        }
        
        public RuntimeEnemy Create(EnemyConfig config)
        {
            if (_pool.PreGet(config.Prototype.name, out var instance))
            {
                instance.Init(config);
                _pool.Occupy(config.Prototype.name, instance);
                return instance;
            }
            
            _pool.Register(config.Prototype.name, () => _instantiator.InstantiatePrefabForComponent<RuntimeEnemy>(config.Prototype, _sceneContainer));
            return Create(config);
        }

        public void Dispose()
        {
            if (_sceneContainer)
                Object.Destroy(_sceneContainer.gameObject);
        }
    }
}