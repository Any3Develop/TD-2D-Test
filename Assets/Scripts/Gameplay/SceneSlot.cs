using Buildings;
using UnityEngine;

namespace Gameplay
{
    public class SceneSlot : MonoBehaviour
    {
        public RuntimeBuilding Building { get; private set; }
        
        public Vector3 Position { get; private set; }
        
        public bool IsFree => !Building;
        
        private void Awake()
        {
            Position = transform.position;
        }

        public bool Occupy(RuntimeBuilding building)
        {
            if (!IsFree)
                return false;
            
            Building = building;
            Building.Container.transform.position = Position;
            return true;
        }

        public void Release()
        {
            if (!IsFree)
                Building.Places.Remove(this);
            
            Building = null;
        }
    }
}