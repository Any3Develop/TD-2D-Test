using Common.Runtime;
using Identity;
using Shop;
using UnityEngine;
using Zenject;

namespace Loots
{
    [RequireComponent(typeof(IRuntimeEntity))]
    public abstract class LootDrop : MonoBehaviour
    {
        [SerializeField] private LootsSet loots;

        [Inject] private readonly ProductsRepository _products;
        [Inject] private readonly UserIdentity _userIdentity;
        
        protected virtual void Drop()
        {
            foreach (var lootItem in loots.Items)
            {
                var productItem = _products.Find(x => x.Id == lootItem.ProductId);
                if (productItem == null)
                    continue;

                switch (productItem.Category)
                {
                    case ItemCategory.Currency :
                    {
                        if (_userIdentity.Wallets.TryGetValue(productItem.ConfigId, out var userWallet))
                            userWallet.Deposit(lootItem.Amount);
                        break;
                    }
                }
            }
        }
    }
}