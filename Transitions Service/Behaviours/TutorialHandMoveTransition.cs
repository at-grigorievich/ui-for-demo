using System;
using ATG.DTO;
using DG.Tweening;
using UnityEngine;

namespace ATG.Transition
{
    public sealed class TutorialHandMoveTransition : ITransitionBehaviour<HandTutorialDTO>
    {
        private readonly TutorialHandMoveTransitionData _config;

        private Sequence _seq;

        public TutorialHandMoveTransition(TutorialHandMoveTransitionData config)
        {
            _config = config;
        }

        public void Execute(HandTutorialDTO slave, Action callback = null)
        {
            if(slave.HandRect == null)
             throw new ArgumentNullException("hand is null");

            Dispose();

            RectTransform rect = slave.HandRect;

            rect.anchoredPosition = slave.Start;

            _seq = DOTween.Sequence()
                .Append(rect.DOAnchorPos(slave.End, _config.Duration))
                .Append(rect.DOAnchorPos(slave.Start, _config.Duration))
                .SetEase(_config.Ease)
                .SetLoops(-1);
            _seq.Play();
        }

        public void Dispose()
        {
            _seq?.Kill();
            _seq = null;
        }
    }
}