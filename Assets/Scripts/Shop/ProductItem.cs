using System;
using UnityEngine;

namespace Shop
{
    [Serializable]
    public class ProductItem
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public ItemCategory Category { get; private set; }
        [field: SerializeField] public string ConfigId { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Title { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public Price[] Price { get; private set; }
    }
}