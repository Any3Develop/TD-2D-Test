using System.Collections.Generic;
using Common.Repositiory;

namespace Gameplay
{
    public class SceneSlotsCollection : Repository<SceneSlot>
    {
        public SceneSlotsCollection(IEnumerable<SceneSlot> slots)
        {
            AddRange(slots);
        }
    }
}