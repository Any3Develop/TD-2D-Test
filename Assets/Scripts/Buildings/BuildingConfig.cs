using System.Collections.Generic;
using Common.Runtime;
using Common.Stats;
using UnityEngine;

namespace Buildings
{
    [CreateAssetMenu(menuName = "TD/Buildings/Building Config", fileName = "Building Config")]
    public class BuildingConfig : ScriptableObject, IEntityConfig
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public GameObject Prototype { get; private set; }
        [field: SerializeField] public List<StatData> Stats { get; private set; } = new();
    }
}