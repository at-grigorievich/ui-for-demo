using System;
using UnityEngine;

namespace ATG.Transition
{
    [CreateAssetMenu(menuName = "configs/transitions/hide vertital ui transition",
                fileName = "new_hide_vertical_ui_transition_config")]
    public sealed class HideVerticalUITransitionData : TweenTransitionData
    {
        public override ITransitionBehaviour<T> Create<T>()
        {
            if (typeof(T) != typeof(RectTransform))
            {
                throw new ArgumentException("T is not RectTransform !");
            }

            return new HideVerticalUITransition(this) as ITransitionBehaviour<T>;
        }
    }
}