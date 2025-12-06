using System.Collections.Generic;
using Common.Repositiory;

namespace Buildings
{
    public class BuildingRepository : Repository<BuildingConfig>
    {
        public BuildingRepository(IEnumerable<BuildingConfig> buildings)
        {
            AddRange(buildings);
        }
    }
}