using Buildings;
using Common.Pools;
using Common.Stats;
using Enemies;
using Gameplay;
using Gameplay.Waves;
using Shop;
using UI.Shop;
using UnityEngine;
using Weapons;
using Zenject;

namespace Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private WaveSpawner spawner;
        [SerializeField] private SceneSlot[] slots;
        [SerializeField] private WaveConfig[] waves;
        
        public override void InstallBindings()
        {
            Container.Bind<StatsCollection>().AsTransient();
            Container.Bind<SceneSlotsCollection>().AsSingle().WithArguments(slots);
            Container.Bind<WavesRepository>().AsSingle().WithArguments(waves);
            Container.BindInstance(spawner).AsSingle();
            
            Container.BindInterfacesTo<UnityPool<RuntimeBuilding>>().AsSingle();
            Container.BindInterfacesTo<UnityPool<RuntimeEnemy>>().AsSingle();
            Container.BindInterfacesTo<UnityPool<Projectile>>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<BuildingFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<BuildingSceneHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<ShopPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<TDGameSetup>().AsSingle().NonLazy();
        }
    }
}