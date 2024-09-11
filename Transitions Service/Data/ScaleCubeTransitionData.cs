using System;
using ATG.MVC;
using UnityEngine;

namespace ATG.Transition
{
    [CreateAssetMenu(menuName = "configs/transitions/excite punch cube transition",
                fileName = "new_excite_punch_cube_transition_config")]
    public sealed class ScaleCubeTransitionData : TweenTransitionData
    {
        [field: SerializeField] public Vector3 Scale;
        
        public override ITransitionBehaviour<T> Create<T>()
        {
            if (typeof(T) != typeof(CubeController))
            {
                throw new ArgumentException("T is not PlacementView !");
            }

            return new ScaleCubeTransition(this) as ITransitionBehaviour<T>;
        }
    }
}