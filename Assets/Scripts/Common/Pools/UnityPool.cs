using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Common.Pools
{
    public class UnityPool<T> : IPool<T> where T : Object
    {
        private class Pool
        {
            public readonly List<T> List = new(32);
            public readonly Func<T> Factory;

            public Pool(Func<T> factory)
            {
                Factory = factory;
            }
        }

        private readonly Dictionary<string, Pool> _pools = new();

        public bool PreGet(string sampleId, out T instance)
        {
            if (string.IsNullOrEmpty(sampleId) || !_pools.TryGetValue(sampleId, out var pool))
            {
                instance = default;
                return false;
            }
            
            if (pool.List.Count > 0)
            {
                instance = pool.List[^1];
                pool.List.RemoveAt(pool.List.Count - 1);
                return true;
            }
            
            instance = pool.Factory();
            return true;
        }

        public void Occupy(string sampleId, T instance)
        {
            if (string.IsNullOrEmpty(sampleId) || !_pools.ContainsKey(sampleId))
                throw new KeyNotFoundException($"Sample '{sampleId}' not registered.");

            CallSpawn(instance);
        }

        public T Get(string sampleId)
        {
            if (string.IsNullOrEmpty(sampleId) || !_pools.TryGetValue(sampleId, out var pool))
            {
                Debug.LogError($"Sample '{sampleId}' not registered.");
                return default;
            }

            T instance;
            if (pool.List.Count > 0)
            {
                instance = pool.List[0];
                pool.List.RemoveAt(0);
            }
            else
            {
                instance = pool.Factory();
            }
            
            CallSpawn(instance);
            return instance;
        }

        public bool TryGet(string sampleId, out T instance)
        {
            if (string.IsNullOrEmpty(sampleId) || !_pools.TryGetValue(sampleId, out _))
            {
                instance = null;
                return false;
            }

            instance = Get(sampleId);
            return true;
        }

        public bool IsRegistered(string sampleId)
        {
            return !string.IsNullOrEmpty(sampleId) && _pools.TryGetValue(sampleId, out _);
        }

        public void Release(string sampleId, T instance)
        {
            if (string.IsNullOrEmpty(sampleId) || !_pools.TryGetValue(sampleId, out var pool))
                return;

            CallRelease(instance);
            pool.List.Add(instance);
        }

        public void Register(string sampleId, Func<T> factory)
        {
            if (_pools.ContainsKey(sampleId))
                return;

            _pools.Add(sampleId, new Pool(factory));
        }

        public void Unregister(string sampleId)
        {
            if (string.IsNullOrEmpty(sampleId) || !_pools.TryGetValue(sampleId, out var pool))
                return;

            pool.List.Clear();
            _pools.Remove(sampleId);
        }

        public void Clear()
        {
            foreach (var key in _pools.Keys)
                _pools[key].List.Clear();

            _pools.Clear();
        }

        public void Dispose() => Clear();

        ~UnityPool() => Clear();

        private static void CallSpawn(T instance)
        {
            switch (instance)
            {
                case GameObject go:
                    foreach (var sp in go.GetComponents<ISpwanPoolable>())
                        sp.Spawn();
                    break;

                case Component comp:
                    foreach (var sp in comp.GetComponents<ISpwanPoolable>())
                        sp.Spawn();
                    break;

                case ISpwanPoolable sp:
                    sp.Spawn();
                    break;
            }
        }

        private static void CallRelease(T instance)
        {
            switch (instance)
            {
                case GameObject go:
                    foreach (var r in go.GetComponents<IPoolable>())
                        r.Release();
                    break;

                case Component comp:
                    foreach (var r in comp.GetComponents<IPoolable>())
                        r.Release();
                    break;

                case IPoolable r:
                    r.Release();
                    break;
            }
        }
    }
}