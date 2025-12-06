using System.Collections.Generic;
using Common.Stats;

namespace Common.Runtime
{
    public interface IEntityConfig
    {
        string Id { get; }
        List<StatData> Stats { get; }
    }
}