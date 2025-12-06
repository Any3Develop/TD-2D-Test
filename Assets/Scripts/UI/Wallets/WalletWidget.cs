using System.Collections.Generic;
using Identity;
using Shop;
using UI.Service;
using UnityEngine;
using Zenject;

namespace UI.Wallets
{
    public class WalletWidget : UIWindow
    {
        [SerializeField] private WalletItem prototype;
        [SerializeField] private RectTransform container;
        
        [Inject] private readonly UserIdentity _userIdentity;
        [Inject] private readonly ProductsRepository _products;
        private readonly Dictionary<string, WalletItem> _items = new();
        
        public override void Open()
        {
            base.Open();
            foreach (var wallet in _userIdentity.Wallets.Values)
            {
                wallet.OnChange += OnWalletChanged;
                if (!_items.TryGetValue(wallet.Id, out var walletItem))
                    _items.Add(wallet.Id, walletItem = Instantiate(prototype, container));

                walletItem.IconImage.sprite = _products.Find(x=> x.ConfigId == wallet.Id && x.Category == ItemCategory.Currency)?.Icon;
                OnWalletChanged(wallet);
            }
        }

        public override void Close()
        {
            base.Close();
            foreach (var wallet in _userIdentity.Wallets.Values)
                wallet.OnChange -= OnWalletChanged;

            foreach (var item in _items.Values)
                Destroy(item);
            
            _items.Clear();
        }

        private void OnWalletChanged(Wallet wallet)
        {
            var item = _items[wallet.Id];
            item.AmountText.SetText(wallet.Amount.ToString());
        }
    }
}