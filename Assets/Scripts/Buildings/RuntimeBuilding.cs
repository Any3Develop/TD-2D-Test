using System;
using System.Collections.Generic;
using Common.Pools;
using Common.Runtime;
using Common.Stats;
using Gameplay;
using UnityEngine;
using Zenject;
using IPoolable = Common.Pools.IPoolable;

namespace Buildings
{
    public class RuntimeBuilding : MonoBehaviour, IPoolable, IRuntimeEntity
    {
        public string Id { get; } = Guid.NewGuid().ToString();
        public GameObject Container { get; private set; }
        [Inject] public StatsCollection Stats { get; private set; }
        [Inject] public IPool<RuntimeBuilding> Pool { get; private set; }
        public BuildingConfig Config { get; private set; }
        public List<SceneSlot> Places { get; } = new();
        
        public virtual void Init(BuildingConfig config)
        {
            Config = config;
            Container = gameObject;
            config.Stats.ForEach(data => Stats.AddStat(data));
            gameObject.SetActive(true);
        }
        
        public virtual void Release()
        {
            Stats.Clear();
            for (var i = Places.Count-1; i >= 0; i--)
                Places[i].Release();
            
            gameObject.SetActive(false);
        }

        public virtual void OnDestroy()
        {
            Release();
            Stats = null;
        }
    }
}