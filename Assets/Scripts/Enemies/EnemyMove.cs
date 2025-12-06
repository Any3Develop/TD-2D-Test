using Common.Pools;
using Common.Stats;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    [RequireComponent(typeof(RuntimeEnemy))]
    public class EnemyMove : MonoBehaviour, ISpwanPoolable
    {
        [SerializeField] private NavMeshAgent agent;
        private RuntimeEnemy _selfEntity;
        private RuntimeStat _moveSpeed; // TODO subscribe to update an agent with new values
        private RuntimeStat _turnSpeed; // TODO subscribe to update an agent with new values
        private EnemyAnimator _animator;
        private bool _initialized;
        public Transform Self { get; private set; }

        private void Awake()
        {
            Self = transform;
            _animator = GetComponent<EnemyAnimator>();
            _selfEntity = GetComponent<RuntimeEnemy>();
        }

        public void Release()
        {
            if (agent.isOnNavMesh) 
                agent.isStopped = true;
            
            agent.enabled = false;
            _initialized = false;
        }

        public void Spawn()
        {
            if (!_selfEntity.Stats.TryGet(x => x.Name == "MoveSpeed", out _moveSpeed))
                _moveSpeed = _selfEntity.Stats.AddStat(new StatData { Name = "MoveSpeed", Value = 2f });
            
            if (!_selfEntity.Stats.TryGet(x => x.Name == "TurnSpeed", out _turnSpeed))
                _turnSpeed = _selfEntity.Stats.AddStat(new StatData{ Name = "TurnSpeed", Value = 2f });

            if (!agent.isOnNavMesh)
                if (NavMesh.SamplePosition(Self.position, out var closestHit, 500f, agent.areaMask))
                    Self.position = closestHit.position;
            
            agent.speed = _moveSpeed.TotalValue;
            agent.angularSpeed = _turnSpeed.TotalValue;
            agent.enabled = true;
        }

        public void SetStartPoint(Vector3 point)
        {
            if (NavMesh.SamplePosition(point, out var closestHit, 500f, agent.areaMask))
                Self.position = closestHit.position;
        }

        public void SetDestination(Vector3 point)
        {
            if (NavMesh.SamplePosition(point, out var closestHit, 500f, agent.areaMask))
            {
                agent.SetDestination(closestHit.position);
                _initialized = true;
            }
        }

        private void Update()
        {
            if (!_initialized)
                return;
            
            if (!_selfEntity.IsAlive)
            {
                _initialized = false;
                if (agent.isOnNavMesh)
                    agent.isStopped = true;
                
                agent.enabled = false;
                return;
            }
            
            if (_animator)
                _animator.Move(agent.velocity.magnitude);
        }
    }
}