using Common.Pools;
using Common.Runtime;
using Common.Stats;
using UnityEngine;

namespace Buildings
{
    [RequireComponent(typeof(IRuntimeEntity))]
    public class TowerRotator : MonoBehaviour, ISpwanPoolable
    {
        [SerializeField] private Transform tower;
        [SerializeField] private float restoreTime = 1f;
        private bool _initialized;
        private Vector3 _defaultDir;
        private IRuntimeEntity _selfEntity;
        private RuntimeStat _turnSpeed;
        private float _restoreDirTimer;

        private void Awake()
        {
            _defaultDir = tower.up;
            _selfEntity = GetComponent<IRuntimeEntity>();
        }

        public void Spawn()
        {
            if (!_selfEntity.Stats.TryGet(x => x.Name == "TurnSpeed", out  _turnSpeed))
                _turnSpeed = _selfEntity.Stats.AddStat(new StatData{Name = "TurnSpeed", Value = 180f});
            
            _initialized = true;
        }

        public void Release()
        {
            _initialized = false;
            _turnSpeed = null;
        }

        public bool RotateToTarget(Vector2 point)
        {
            if (!_initialized)
                return false;
            
            _restoreDirTimer = Time.time + restoreTime;
            return RotateToDir(point - (Vector2)tower.position);
        }

        public bool RotateToDir(Vector2 dir)
        {
            if (!_initialized)
                return false;
            
            _restoreDirTimer = Time.time + restoreTime;
            return RotateDirInternal(dir);
        }

        private bool RotateDirInternal(Vector2 dir)
        {
            if (!_initialized)
                return false;

            var targetZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f; 
            var currentZ = tower.eulerAngles.z;

            var newZ = Mathf.MoveTowardsAngle(currentZ, targetZ, _turnSpeed.TotalValue * Time.deltaTime);
            tower.rotation = Quaternion.Euler(0, 0, newZ);

            return Mathf.Abs(Mathf.DeltaAngle(newZ, targetZ)) < 0.1f;
        }

        private void Update()
        {
            if(!_initialized || _restoreDirTimer > Time.time)
                return;

            if (RotateDirInternal(_defaultDir))
                _restoreDirTimer = Time.time + 5f; // more time after the end, fewer updates
        }
    }
}