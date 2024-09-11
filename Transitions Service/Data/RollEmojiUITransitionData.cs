using System;
using UnityEngine;

namespace ATG.Transition
{
    [CreateAssetMenu(menuName = "configs/transitions/roll emoji ui transition",
                fileName = "new_roll_emoji_ui_transition_config")]
    public sealed class RollEmojiUITransitionData : TweenTransitionData
    {        
        public override ITransitionBehaviour<T> Create<T>()
        {
            if (typeof(T) != typeof(RectTransform))
            {
                throw new ArgumentException("T is not RectTransform !");
            }

            return new RollEmojiUITransition(this) as ITransitionBehaviour<T>;
        }
    }
}