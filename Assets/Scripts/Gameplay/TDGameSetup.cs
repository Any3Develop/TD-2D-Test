using System.Linq;
using Buildings;
using Identity;
using Shop;
using UI.Service;
using UI.Shop;
using UI.Wallets;
using Zenject;

namespace Gameplay
{
    public class TDGameSetup : IInitializable
    {
        private readonly UserIdentity _userIdentity;
        private readonly ProductsRepository _products;
        private readonly BuildingRepository _buildings;
        private readonly BuildingSceneHandler _buildingSceneHandler;
        private readonly ShopPresenter _shopPresenter;
        private readonly IUIService _uiService;

        public TDGameSetup(
            UserIdentity userIdentity,
            ProductsRepository products,
            BuildingSceneHandler buildingSceneHandler,
            BuildingRepository buildings,
            ShopPresenter shopPresenter,
            IUIService uiService)
        {
            _userIdentity = userIdentity;
            _products = products;
            _buildings = buildings;
            _buildingSceneHandler = buildingSceneHandler;
            _shopPresenter = shopPresenter;
            _uiService = uiService;
        }

        public void Initialize()
        {
            foreach (var product in _products.Where(x=> x.Category == ItemCategory.Currency))
                _userIdentity.Wallets.Add(product.Id, new Wallet(product.Id));

            const int freeBuildings = 2;
            for (var i = 0; i < freeBuildings; i++)
                _buildingSceneHandler.Build(_buildings[i % freeBuildings], null);
            
            _uiService.Open<WalletWidget>();
            _shopPresenter.Open();
        }
    }
}