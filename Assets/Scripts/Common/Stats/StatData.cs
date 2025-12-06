using System;

namespace Common.Stats
{
    [Serializable]
    public struct StatData
    {
        public string Name;
        public float Value;
        public float Previous;
    }
}