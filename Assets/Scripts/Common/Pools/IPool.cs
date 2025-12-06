using System;

namespace Common.Pools
{
    public interface IPool<T> : IDisposable
    {
        bool PreGet(string sampleId, out T instance);
        
        void Occupy(string sampleId, T instance);
            
        bool TryGet(string sampleId, out T instance);
        
        bool IsRegistered(string sampleId);

        void Register(string sampleId, Func<T> factory);

        void Unregister(string sampleId);

        void Clear();

        T Get(string sampleId);

        void Release(string sampleId, T instance);
    }
}