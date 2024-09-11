using DG.Tweening;
using UnityEngine;

namespace ATG.Transition
{
    public abstract class TweenTransitionData : TransitionData
    {
        [field: SerializeField] public float Duration { get; private set; }
        [field: SerializeField] public Ease Ease { get; private set; }
    }
}