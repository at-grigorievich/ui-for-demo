using System;
using ATG.DTO;
using UnityEngine;

namespace ATG.Transition
{
    [CreateAssetMenu(menuName = "configs/transitions/scale button transition",
               fileName = "scale_button_transition_config")]
    public sealed class ScaleButtonTransitionData : TweenTransitionData
    {
        public override ITransitionBehaviour<T> Create<T>()
        {
            if (typeof(T) != typeof(ScaleButtonDTO))
            {
                throw new ArgumentException("T is not GraphicButtonDTO !");
            }

            return new ScaleButtonTransition(this) as ITransitionBehaviour<T>;
        }
    }
}