using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Gameplay.Waves
{
    [CreateAssetMenu(menuName = "TD/Waves/Wave Config", fileName = "New Wave Config")]
    public class WaveConfig : ScriptableObject
    {
        [field: SerializeField] public int Level { get; private set; }
        [field: SerializeField] public float DelayStart { get; private set; }
        [field: SerializeField] public List<EnemyConfig> Enemies { get; private set; }
    }
}