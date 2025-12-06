using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Utils
{
    public class TriggerAccessor : MonoBehaviour
    {
        protected readonly Dictionary<object, Action<Collider2D>> OnEnterCallbacks = new();
        protected readonly Dictionary<object, Action<Collider2D>> OnExitCallbacks = new();
        
        public void AddOnEnterCallback(Action<Collider2D> callback)
        {
            var obj = callback.Target;
            if (OnEnterCallbacks.TryGetValue(callback.Target, out var c))
            {
                c += callback;
            } 
            else
            {
                c = callback;
            }

            OnEnterCallbacks[obj] = c;
        }
        
        public void AddOnExitCallback(Action<Collider2D> callback)
        {
            var obj = callback.Target;
            if (OnExitCallbacks.TryGetValue(callback.Target, out var c))
            {
                c += callback;
            } 
            else
            {
                c = callback;
            }

            OnExitCallbacks[obj] = c;
        }

        public void RemoveOnEnterCallback(Action<Collider2D> callback)
        {
            var obj = callback.Target;
            if (OnEnterCallbacks.TryGetValue(obj, out var c))
            {
                c -= callback;
            } 
            OnEnterCallbacks[obj] = c;
        }

        public void RemoveOnEnterCallbacks(object obj)
        {
            if (OnEnterCallbacks.ContainsKey(obj))
            {
                OnEnterCallbacks.Remove(obj);
            }
        }
        
        public void RemoveOnExitCallback(Action<Collider2D> callback)
        {
            var obj = callback.Target;
            if (OnExitCallbacks.TryGetValue(obj, out var c))
            {
                c -= callback;
            } 
            OnExitCallbacks[obj] = c;
        }
        
        public void RemoveOnExitCallbacks(object obj)
        {
            if (OnExitCallbacks.ContainsKey(obj))
            {
                OnExitCallbacks.Remove(obj);
            }
        }

        public void RemoveAllCallbacks(object obj)
        {
            RemoveOnEnterCallbacks(obj);
            RemoveOnExitCallbacks(obj);
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            foreach (var action in OnEnterCallbacks.Values)
            {
                action.Invoke(other);
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            foreach (var action in OnExitCallbacks.Values)
            {
                action.Invoke(other);
            }
        }
    }
}