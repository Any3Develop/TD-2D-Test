using System.Collections.Generic;
using Buildings;
using Identity;
using Shop;
using UI.Service;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private UIRoot uiRootPrefab;
        [SerializeField] private List<UIWindow> uiWindowPrefabs;
        [SerializeField] private ProductsSet[] products;
        [SerializeField] private BuildingConfig[] buildings;
        
        public override void InstallBindings()
        {
            Container.Bind<ProductsRepository>().AsSingle().WithArguments(products);
            Container.Bind<BuildingRepository>().AsSingle().WithArguments(buildings);
            Container.Bind<UserIdentity>()
                .AsSingle();
            
            Container.Bind<UIRoot>()
                .FromComponentInNewPrefab(uiRootPrefab)
                .AsSingle();
            
            Container.BindInterfacesTo<SimpleUIService>()
                .AsSingle()
                .WithArguments(uiWindowPrefabs)
                .NonLazy();
        }
    }
}