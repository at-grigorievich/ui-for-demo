using System;
using DG.Tweening;
using UnityEngine;

namespace ATG.Transition
{
    [CreateAssetMenu(menuName = "configs/transitions/change text output transition",
                fileName = "new_change_text_output_transition_config")]
    public sealed class ChangeTextOutputTransitionData : TransitionData
    {   
        [field: SerializeField] public Vector3 DissolveScale {get; private set;} = Vector3.zero;
        [field: SerializeField] public float DissolveDuration {get; private set;} = 0.5f;
        [field: SerializeField] public Ease DissolveEase {get; private set;} = Ease.Linear;
        
        [field: Space(15)]

        [field: SerializeField] public Vector3 AppearanceScale {get; private set;} = Vector3.one;
        [field: SerializeField] public float AppearanceDuration {get; private set;} = 0.5f;
        [field: SerializeField] public Ease AppearanceEase {get; private set;} = Ease.Linear;

        [field: Space(15)]
        
        [field: SerializeField] public Vector3 PunchScale {get; private set;} = Vector3.one;
        [field: SerializeField] public float PunchDuration {get; private set;} = 0.5f;
        [field: SerializeField] public int Vibrato {get; private set;} = 2;
        [field: SerializeField] public float Elasticy {get; private set;} = 1f;
        [field: SerializeField] public Ease PunchEase {get; private set;} = Ease.Linear;

        public override ITransitionBehaviour<T> Create<T>()
        {
            if (typeof(T) != typeof(RectTransform))
            {
                throw new ArgumentException("T is not PlacementView !");
            }

            return new ChangeTextOutputTransition(this) as ITransitionBehaviour<T>;
        }
    }
}