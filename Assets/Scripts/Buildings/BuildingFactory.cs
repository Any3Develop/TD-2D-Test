using System;
using Common.Pools;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Buildings
{
    public class BuildingFactory : IDisposable
    {
        private readonly Transform _sceneContainer;
        private readonly IPool<RuntimeBuilding> _pool;
        private readonly IInstantiator _instantiator;

        public BuildingFactory(
            IPool<RuntimeBuilding> pool,
            IInstantiator instantiator)
        {
            this._pool = pool;
            this._instantiator = instantiator;
            _sceneContainer = new GameObject("Buildings").transform;
        }
        
        public RuntimeBuilding Create(BuildingConfig config)
        {
            if (_pool.PreGet(config.Prototype.name, out var instance))
            {
                instance.Init(config);
                _pool.Occupy(config.Prototype.name, instance);
                return instance;
            }
            
            _pool.Register(config.Prototype.name, () => _instantiator.InstantiatePrefabForComponent<RuntimeBuilding>(config.Prototype, _sceneContainer));
            return Create(config);
        }

        public void Dispose()
        {
            if (_sceneContainer)
                Object.Destroy(_sceneContainer.gameObject);
        }
    }
}