using System;
using UnityEngine;
using VContainer;

namespace ATG.UI
{
    [Serializable]
    public sealed class UserInterfaceServiceFactory
    {
        [SerializeField] private UIViewSetData config;
        [SerializeField] private Transform viewsParent;

        public void Create(IContainerBuilder builder)
        {
            builder.Register<IUserInterfaceService, UserInterfaceService>(Lifetime.Singleton)
                .WithParameter<UIViewSetData>(config)
                .WithParameter<Transform>(viewsParent);
        }
    }
}