using System;
using ATG.MVC;
using UnityEngine;

namespace ATG.Transition
{
    [CreateAssetMenu(menuName = "configs/transitions/hide cube transition",
                fileName = "hide_cube_transition_config")]
    public sealed class HideCubeTransitionData: TweenTransitionData
    {
        [field: SerializeField] public Vector3 HideScale {get; private set;}

        public override ITransitionBehaviour<T> Create<T>()
        {
            if (typeof(T) != typeof(CubeController))
            {
                throw new ArgumentException("T is not CubeController !");
            }

            return new HideCubeTransition(this) as ITransitionBehaviour<T>;
        }
    }
}