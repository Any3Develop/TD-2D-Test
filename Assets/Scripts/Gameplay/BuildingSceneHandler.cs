using System;
using System.Linq;
using Buildings;

namespace Gameplay
{
    public enum BuildResult
    {
        Success,
        Cancelled,
    }
    
    public class BuildingSceneHandler
    {
        private readonly SceneSlotsCollection _slotsCollection;
        private readonly BuildingFactory _buildingFactory;

        public BuildingSceneHandler(
            SceneSlotsCollection slotsCollection, 
            BuildingFactory buildingFactory)
        {
            _slotsCollection = slotsCollection;
            _buildingFactory = buildingFactory;
        }

        public void Build(BuildingConfig config, Action<BuildResult> result)
        {
            var slot = _slotsCollection.FirstOrDefault(x => x.IsFree);
            if (slot == null || config == null)
            {
                result?.Invoke(BuildResult.Cancelled);
                return;
            }

            var building = _buildingFactory.Create(config);
            slot.Occupy(building);
            result?.Invoke(BuildResult.Success);
        }
    }
}