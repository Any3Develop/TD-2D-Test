using System.Collections.Generic;
using UI.Service;
using UnityEngine;

namespace UI.Shop
{
    public class ShopWindow : UIWindow
    {
        [SerializeField] private ShopItem prototype;
        [SerializeField] private RectTransform container;
        public List<ShopItem> Items { get; private set; } = new();

        public ShopItem AddItem()
        {
            var item = Instantiate(prototype, container);
            Items.Add(item);
            return item;
        }

        public void RemoveItem(ShopItem item)
        {
            if (!item)
                return;
            
            Items.Remove(item);
            Destroy(item);
        }

        public void Clear()
        {
            foreach (var item in Items)
                RemoveItem(item);
        }

        public override void Close()
        {
            base.Close();
            Clear();
        }
    }
}