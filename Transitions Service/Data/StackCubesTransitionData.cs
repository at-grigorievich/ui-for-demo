using System;
using ATG.DTO;
using UnityEngine;

namespace ATG.Transition
{
    [CreateAssetMenu(menuName = "configs/transitions/stack cubes transition",
            fileName = "new_stack_cubes_transition_config")]
    public sealed class StackCubesTransitionData : TweenTransitionData
    {
        [field: SerializeField] public float DelayBeforeStack { get; private set; }

        public override ITransitionBehaviour<T> Create<T>()
        {
            if (typeof(T) != typeof(StackDTO))
            {
                throw new ArgumentException("T is not PlacementView !");
            }

            return new StackCubesTransition(this) as ITransitionBehaviour<T>;
        }
    }
}