using System;
using Common.Pools;
using UnityEngine;

namespace Common.Stats
{
    public class RuntimeStat : IPoolable
    {
        public event Action<RuntimeStat> OnChaged;
        public string Id { get; private set; }
        public string Name { get; private set; }
        public float Value { get; private set; }

        public float TotalValue => Value + Modifier;
        public float Previous { get; private set; }
        public float Modifier { get; set; } // TODO extend with IStatModifier and calculate them into this value
        
        public void Init(string id, string name, float value, float previous = 0, float modifier = 0)
        {
            Id = id;
            Name = name;
            Value = value;
            Modifier = modifier;
            Previous = previous;
        }

        public void Release()
        {
            OnChaged = null;
        }

        public void SetValue(float value, bool notify = true)
        {
            if (Mathf.Approximately(value, Value))
                return;
            
            Previous = Value;
            Value = value;
            
            if (notify)
                OnChaged?.Invoke(this);
        }
    }
}