using System;
using Common.Pools;
using Enemies;
using UnityEngine;

namespace Weapons
{
    public class Projectile : MonoBehaviour, ISpwanPoolable
    {
        public bool RotateToTarget = true;
        [HideInInspector] public Vector3 Destination;
        [HideInInspector] public Transform SteeringTarget;
        [HideInInspector] public Transform Self;
        [HideInInspector] public GameObject Container;
        
        public float Speed = 5f;
        public Action OnHitCallback;
        protected bool Initialized;

        private void Awake()
        {
            Self = transform;
            Container = gameObject;
        }

        public virtual void Spawn()
        {
            Initialized = true;
            Container.SetActive(true);
        }
        
        public virtual void Release()
        {
            Initialized = false;
            SteeringTarget = null;
            Destination = Vector3.zero;
            Container.SetActive(false);
        }

        public virtual RuntimeEnemy[] GetHitTargets() => null;

        protected virtual Vector3 GetTargetPosition() 
            => SteeringTarget ? SteeringTarget.position : Destination;

        protected virtual void Update()
        {
            if (!Initialized)
                return;
            
            var targetPos = GetTargetPosition();
            var dir = targetPos - Self.position;
            var dist = dir.magnitude;

            if (dist < 0.01f)
            {
                Hit();
                return;
            }

            var move = Speed * Time.deltaTime;

            if (move >= dist)
            {
                Self.position = targetPos;
                Hit();
            }
            else
            {
                Self.position += dir.normalized * move;
            }
            
            if (RotateToTarget)
            {
                var euler = transform.eulerAngles;
                euler.z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f; 
                Self.rotation = Quaternion.Euler(euler);
            }
        }

        protected virtual void Hit()
        {
            Initialized = false;
            OnHitCallback?.Invoke();
            OnHitCallback = null;
        }
    }
}