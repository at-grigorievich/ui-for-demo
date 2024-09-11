using System;
using ATG.MVC;
using UnityEngine;

namespace ATG.Transition
{
    [CreateAssetMenu(menuName = "configs/transitions/spawn cube transition",
                fileName = "new_spawn_cube_transition_config")]
    public sealed class SpawnCubeTransitionData : TweenTransitionData
    {
        [field: SerializeField] public Vector3 InitScale {get; private set;}
        [field: SerializeField] public Vector3 ResultScale {get; private set;}
        [field: SerializeField] public Vector3 PunchPower {get; private set;}
        [field: SerializeField, Range(1,20)] public int Vibrato {get; private set;}
        [field: SerializeField,Range(0f, 1f)] public float Elasticy {get; private set;}

        public override ITransitionBehaviour<T> Create<T>()
        {
            if (typeof(T) != typeof(CubeController))
            {
                throw new ArgumentException("T is not PlacementView !");
            }

            return new SpawnCubeTransition(this) as ITransitionBehaviour<T>;
        }
    }
}