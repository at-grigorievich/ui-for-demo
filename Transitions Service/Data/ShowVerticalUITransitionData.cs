using System;
using UnityEngine;

namespace ATG.Transition
{
    [CreateAssetMenu(menuName = "configs/transitions/show vertital ui transition",
                fileName = "new_show_vertical_ui_transition_config")]
    public sealed class ShowVerticalUITransitionData : TweenTransitionData
    {        
        public override ITransitionBehaviour<T> Create<T>()
        {
            if (typeof(T) != typeof(RectTransform))
            {
                throw new ArgumentException("T is not RectTransform !");
            }

            return new ShowVerticalUITransition(this) as ITransitionBehaviour<T>;
        }
    }
}