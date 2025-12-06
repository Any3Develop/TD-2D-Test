using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common.Pools;
using Common.Runtime;
using DG.Tweening;
using Enemies;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay.Waves
{
    public class WaveSpawner : MonoBehaviour
    {
        [SerializeField] private List<WavePoint> spawnPoints;
        [SerializeField] private List<WavePoint> targetPoints;

        [Inject] private readonly IPool<RuntimeEnemy> _enemyPool;
        [Inject] private readonly WavesRepository _repository;
        [Inject] private readonly EnemyFactory _factory;
        
        private WaveConfig _waveConfig;
        public int EnemyLeft;
        public int Level;
        public int LevelMax;

        private void Start()
        {
            LevelMax = _repository.Max(x => x.Level);
            TryToStartNewWave();
        }

        private IEnumerator StartWave()
        {
            var waitSeconds = new WaitForSeconds(0.5f);
            foreach (var enemyConfig in _waveConfig.Enemies)
            {
                yield return waitSeconds;
                var runtimeEnemy = _factory.Create(enemyConfig);
                if (!runtimeEnemy.Container.TryGetComponent(out EnemyMove move))
                {
                    runtimeEnemy.Container.transform.position = spawnPoints[Random.Range(0, spawnPoints.Count)].Point;
                    continue;
                }

                move.SetStartPoint(spawnPoints[Random.Range(0, spawnPoints.Count)].Point);
                move.SetDestination(targetPoints[Random.Range(0, targetPoints.Count)].Point);
            }
        }

        public void OnPassed(IRuntimeEntity entity)
        {
            EnemyLeft--;
            TryToStartNewWave();
        }

        public void OnDied(IRuntimeEntity entity)
        {
            EnemyLeft--;
            TryToStartNewWave();
        }

        private void TryToStartNewWave()
        {
            if (EnemyLeft > 0)
                return;

            Level = Mathf.Min(Level + 1, LevelMax);
            _waveConfig = _repository.Get(Level);
            EnemyLeft = _waveConfig.Enemies.Count;

            DOVirtual.DelayedCall(_waveConfig.DelayStart, () => StartCoroutine(StartWave()));
        }
    }
}