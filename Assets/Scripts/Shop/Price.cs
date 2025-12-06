using System;
using UnityEngine;

namespace Shop
{
    [Serializable]
    public class Price
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public int Amount { get; private set; }
    }
}