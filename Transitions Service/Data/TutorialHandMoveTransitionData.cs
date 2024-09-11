using System;
using ATG.DTO;
using UnityEngine;

namespace ATG.Transition
{
    [CreateAssetMenu(menuName = "configs/transitions/tutorial hand transition",
            fileName = "new_tutorial_hand_transition_config")]
    public sealed class TutorialHandMoveTransitionData : TweenTransitionData
    {
        public override ITransitionBehaviour<T> Create<T>()
        {
            if (typeof(T) != typeof(HandTutorialDTO))
            {
                throw new ArgumentException("T is not HandTutorialDTO !");
            }

            return new TutorialHandMoveTransition(this) as ITransitionBehaviour<T>;
        }
    }
}