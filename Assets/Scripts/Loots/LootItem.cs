using System;
using UnityEngine;

namespace Loots
{
	[Serializable]
    public class LootItem
    {
        [field: SerializeField] public string ProductId { get; private set; }
        [field: SerializeField] public int Amount { get; private set; }
    }
}