using System;
using ATG.MVC;
using UnityEngine;

namespace ATG.Transition
{
    [CreateAssetMenu(menuName = "configs/transitions/excite placement transition",
                    fileName = "new_height_transition_config")]
    public sealed class HeightPlacementTransitionData : TweenTransitionData
    {
        [field: SerializeField] public float NeedHeight { get; private set; }

        public override ITransitionBehaviour<T> Create<T>()
        {
            if(typeof(T) != typeof(PlacementView))
            {
                throw new ArgumentException("T is not PlacementView !");
            }

            return new HeightPlacementTransition(this) as ITransitionBehaviour<T>;
        }
    }
}