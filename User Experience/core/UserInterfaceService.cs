using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

using Resolver = VContainer.IObjectResolver;
using Debug = UnityEngine.Debug;

namespace ATG.UI
{
    //TODO: Inject CancellationToken into async methods -_-
    public sealed class UserInterfaceService : IUserInterfaceService
    {
        public static UserInterfaceDIResolver DIResolver { get; private set; }

        private readonly UIViewSetData _config;
        private readonly Transform _viewsParent;

        private readonly IReadOnlyDictionary<UIElementType, UIViewSetup> _configDicitionary;
        private readonly ConcurrentDictionary<UIElementType, UIView> _instancesDictionary;

        public bool IsActive { get; private set; }

        public UserInterfaceService(Resolver resolver, UIViewSetData config, Transform viewParent)
        {
            DIResolver = new UserInterfaceDIResolver(resolver);

            _config = config;
            _viewsParent = viewParent;

            _configDicitionary = new Dictionary<UIElementType, UIViewSetup>(_config.ViewPrefabs);
            _instancesDictionary = new ConcurrentDictionary<UIElementType, UIView>();
        }

        public void SetActive(bool isActive)
        {
            IsActive = isActive;

            foreach (var instance in _instancesDictionary.Values)
            {
                instance.SetActive(isActive);
            }
        }

        #region  Get view
        public UIView GetViewByType(UIElementType type)
        {
            if (_instancesDictionary.ContainsKey(type)) return _instancesDictionary[type];
            return CreateInstance(type);
        }

        public async UniTask<UIView> GetViewByTypeAsync(UIElementType type)
        {
            if (_instancesDictionary.ContainsKey(type)) return _instancesDictionary[type];
            return await CreateInstanceAsync(type);
        }
        #endregion

        #region  Show/Hide view
        public UIView ShowViewByType(UIElementType type, object sender, object data)
        {
            UIView view = GetViewByType(type);
            view.Show(sender, data);

            return view;
        }
        public async UniTask<UIView> ShowViewByTypeAsync(UIElementType type, object sender, object data, 
            bool withOpennedDelay = false)
        {
            UIView view = await GetViewByTypeAsync(type);
            
            if(withOpennedDelay == true)
            {
                await UniTask.Delay(_config.ShowViewDelayMilliseconds);
            }

            view.Show(sender, data);

            return view;
        }
        public void HideViewByType(UIElementType type)
        {
            if (_instancesDictionary.ContainsKey(type) == false) return;

            UIView view = _instancesDictionary[type];
            view.Hide();
        }

        public async UniTask HideViewByTypeWithDelay(UIElementType type)
        {
            await UniTask.Delay(_config.ShowViewDelayMilliseconds);
            HideViewByType(type);
        }
        #endregion

        public void ChangeViewLayerIndexByType(UIElementType type, int layerIndex)
        {
            if (HasInstances(type) == true)
            {
                _instancesDictionary[type].ChangeLayerIndex(layerIndex);
                return;
            }
            Debug.LogWarning($"ui view = {type} is required but not instanced !");
        }

        #region  Create view instance
        private UIView CreateInstance(UIElementType type)
        {
            if (HasConfig(type) == false)
                throw new NullReferenceException($"config with type={type} not exist in set!");

            UIViewSetup setup = _configDicitionary[type];

            var instance = GameObject.Instantiate<UIView>(setup.Prefab, _viewsParent);

            _instancesDictionary.TryAdd(type, instance);

            ChangeViewLayerIndexByType(type, setup.SortOrder);

            instance.SetActive(false);

            return instance;
        }
        private async UniTask<UIView> CreateInstanceAsync(UIElementType type)
        {
            if (HasConfig(type) == false)
                throw new NullReferenceException($"config with type={type} not exist in set!");

            UIViewSetup setup = _configDicitionary[type];

            AsyncInstantiateOperation<UIView> operation =
                GameObject.InstantiateAsync(setup.Prefab);

            await operation;

            UIView instance = operation.Result[0];

            if (_instancesDictionary.TryAdd(type, instance))
            {
                ChangeViewLayerIndexByType(type, setup.SortOrder);
                instance.SetActive(false);

                return instance;
            }
            else throw new InvalidOperationException("Can't add ui view instance to instances dictionary");
        }
        #endregion

        private bool HasConfig(UIElementType type) => _configDicitionary.ContainsKey(type);
        private bool HasInstances(UIElementType type) => _instancesDictionary.ContainsKey(type);
    }
}