using System;
using UnityEngine;

namespace Loots
{
    [CreateAssetMenu(menuName = "TD/Loots/Loots Set",fileName = "New Loots Set")]
    public class LootsSet : ScriptableObject
    {
        [field: SerializeField] public LootItem[] Items { get; private set; } = Array.Empty<LootItem>();
    }
}