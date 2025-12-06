using System.Collections.Generic;
using Common.Runtime;
using Common.Stats;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(menuName = "TD/Enemies/Enemy Config", fileName = "New Enemy Config")]
    public class EnemyConfig : ScriptableObject, IEntityConfig
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public GameObject Prototype { get; private set; }
        [field: SerializeField] public List<StatData> Stats { get; private set; } = new();
    }
}