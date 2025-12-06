using System;
using UnityEngine;

namespace Shop
{
    [CreateAssetMenu(menuName = "TD/Shop/Products Set", fileName = "New Product Set")]
    public class ProductsSet : ScriptableObject
    {
        [field: SerializeField] public ProductItem[] Items { get; private set; } = Array.Empty<ProductItem>();
    }
}