using System.Linq;
using Buildings;
using Gameplay;
using Identity;
using Shop;
using UI.Service;

namespace UI.Shop
{
    public class ShopPresenter
    {
        private readonly IUIService _uiService;
        private readonly UserIdentity _userIdentity;
        private readonly ProductsRepository _products;
        private readonly BuildingRepository _buildings;
        private readonly BuildingSceneHandler _buildingSceneHandler;
        private ShopWindow _window;

        public ShopPresenter(
            IUIService uiService,
            UserIdentity userIdentity,
            ProductsRepository products, 
            BuildingRepository buildings,
            BuildingSceneHandler buildingSceneHandler)
        {
            _uiService = uiService;
            _userIdentity = userIdentity;
            _products = products;
            _buildings = buildings;
            _buildingSceneHandler = buildingSceneHandler;
        }

        public void Open()
        {
            _window = _uiService.Open<ShopWindow>();
            foreach (var wallet in _userIdentity.Wallets.Values)
                wallet.OnChange += OnWalletChanged;

            var currencies = _products.
                Where(x => x.Category == ItemCategory.Currency)
                .ToDictionary(x => x.Id);
            
            foreach (var product in _products.Where(x => x.Category == ItemCategory.Towers))
            {
                var shopItem = _window.AddItem();
                shopItem.Product = product;
                shopItem.BuyButton.onClick.AddListener(() => OnItemBuy(shopItem));
                shopItem.IconImage.sprite = product.Icon;
                
                foreach (var price in product.Price)
                    shopItem.AddPrice(price.Amount.ToString(), currencies[price.Id].Icon);
                
                UpdateItemInteractable(shopItem);
            }
        }

        private void UpdateItemInteractable(ShopItem shopItem)
        {
            foreach (var price in shopItem.Product.Price)
            {
                if (!_userIdentity.Wallets.TryGetValue(price.Id, out var wallet)) 
                    continue;
                
                if (wallet.Amount >= price.Amount != shopItem.BuyButton.interactable)
                    shopItem.BuyButton.interactable = !shopItem.BuyButton.interactable;
            }
        }
        
        public void Close()
        {
            foreach (var wallet in _userIdentity.Wallets.Values)
                wallet.OnChange -= OnWalletChanged;
            
            _window = null;
            _uiService.Close<ShopWindow>();
        }
        
        private void OnWalletChanged(Wallet wallet)
        {
            foreach (var shopItem in _window.Items)
                UpdateItemInteractable(shopItem);
        }

        private void OnItemBuy(ShopItem item)
        {
            var buildConfig = _buildings.Find(x => x.Id == item.Product.ConfigId);
            _buildingSceneHandler.Build(buildConfig, result =>
            {
                if (result == BuildResult.Cancelled)
                    return;

                foreach (var price in item.Product.Price)
                    _userIdentity.Wallets[price.Id].Withdraw(price.Amount);
            });
        }
    }
}