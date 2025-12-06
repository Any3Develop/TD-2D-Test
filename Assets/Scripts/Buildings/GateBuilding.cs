using Common.Utils;
using Enemies;
using Gameplay.Waves;
using UnityEngine;
using Zenject;

namespace Buildings
{
    public class GateBuilding : MonoBehaviour
    {
        [SerializeField] private TriggerAccessor enemyTrigger;
        [Inject] private readonly WaveSpawner _spawner;

        private void Awake()
        {
            enemyTrigger.AddOnEnterCallback(c =>
            {
                if (!c.TryGetComponent(out RuntimeEnemy enemy))
                    return;
         
                enemy.SelfRelease();
                _spawner.OnPassed(enemy);
            });
        }

        private void OnDestroy()
        {
            enemyTrigger.RemoveAllCallbacks(this);
        }
    }
}