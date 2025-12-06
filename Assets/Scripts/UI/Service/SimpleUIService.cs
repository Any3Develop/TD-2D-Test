using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace UI.Service
{
    public class SimpleUIService : IUIService
    {
        private readonly UIRoot _uiRoot;
        private readonly IInstantiator _instantiator;
        private readonly List<UIWindow> _windowPrefabs;
        private readonly Dictionary<Type, UIWindow> _windows = new();

        public SimpleUIService(
            UIRoot uiRoot, 
            IInstantiator instantiator, 
            IEnumerable<UIWindow> prefabs)
        {
            _uiRoot = uiRoot;
            _instantiator = instantiator;
            _windowPrefabs = prefabs.ToList();
        }

        public T Open<T>() where T : UIWindow
        {
            var window = Get<T>();
            window.Open();
            return window;
        }

        public void Close<T>() where T : UIWindow
        {
            if (_windows.TryGetValue(typeof(T), out var window))
                window.Close();
        }

        public T Get<T>() where T : UIWindow
        {
            var type = typeof(T);

            if (_windows.TryGetValue(type, out var wnd))
                return (T) wnd;

            foreach (var prefab in _windowPrefabs)
            {
                if (prefab.GetType() == type)
                {
                    var instance = _instantiator.InstantiatePrefab(prefab, prefab.Dynamic ? _uiRoot.DynamicCanvas.transform : _uiRoot.StaticCanvas.transform).GetComponent<T>();
                    instance.gameObject.SetActive(false);
                    _windows[type] = instance;
                    return instance;
                }
            }

            Debug.LogError($"[UIService] Prefab for window {type.Name} not found!");
            return null;
        }

        public bool IsOpen<T>() where T : UIWindow
        {
            return _windows.TryGetValue(typeof(T), out var wnd) && wnd.IsOpened;
        }
    }
}