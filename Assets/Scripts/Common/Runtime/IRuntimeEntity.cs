using Common.Stats;
using UnityEngine;

namespace Common.Runtime
{
    public interface IRuntimeEntity
    {
        string Id { get; }
        GameObject Container { get; }
        StatsCollection Stats { get; }
    }
}