using System;
using ATG.DTO;
using UnityEngine;

namespace ATG.Transition
{
    [CreateAssetMenu(menuName = "configs/transitions/press button transition",
            fileName = "press_button_transition_config")]
    public sealed class PressButtonTransitionData : TransitionData
    {
        [field: SerializeField] public Vector2 MainRectDefaultTopBottom { get; private set;}
        [field: SerializeField] public Vector2 BackgorundDefaultTopBottom { get; private set; }
        [field: SerializeField] public Vector2 ContentDefaultTopBottom { get; private set;}
        [field: Space(15)]
        [field: SerializeField] public Vector2 MainRectPressTopBottom { get; private set;}
        [field: SerializeField] public Vector2 BackgorundPressTopBottom { get; private set; }
        [field: SerializeField] public Vector2 ContentPressTopBottom { get; private set;}

        public override ITransitionBehaviour<T> Create<T>()
        {
            if (typeof(T) != typeof(GraphicButtonDTO))
            {
                throw new ArgumentException("T is not GraphicButtonDTO !");
            }

            return new PressButtonTransition(this) as ITransitionBehaviour<T>;
        }
    }
}