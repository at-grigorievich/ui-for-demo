using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace ATG.Transition
{
    [Serializable]
    public sealed class TransitionServiceFactory
    {
        [SerializeField] private TransitionData[] transitionsSet;

        public void Create(IContainerBuilder builder)
        {
            builder.Register<ITransitionService, TransitionService>(Lifetime.Singleton)
        .WithParameter<IReadOnlyCollection<TransitionData>>(transitionsSet);
        }
    }
}