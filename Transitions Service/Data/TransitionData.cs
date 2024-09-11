using UnityEngine;

namespace ATG.Transition
{
    public abstract class TransitionData : ScriptableObject, ITransitionData
    {
        [field: SerializeField] public TransitionType Type { get; private set; }

        public abstract ITransitionBehaviour<T> Create<T>();
    }
}