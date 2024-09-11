using System;
using DG.Tweening;
using UnityEngine;

namespace ATG.Transition
{
    public sealed class ChangeTextOutputTransition : ITransitionBehaviour<RectTransform>
    {
        private readonly ChangeTextOutputTransitionData _config;
        private Sequence _seq;

        public ChangeTextOutputTransition(ChangeTextOutputTransitionData config)
        {
            _config = config;
        }

        
        public void Execute(RectTransform slave, Action callback = null)
        {
            Dispose();

            _seq = DOTween.Sequence()
                .Append(slave.DOScale(_config.DissolveScale, _config.DissolveDuration).SetEase(_config.DissolveEase))
                .Append(slave.DOScale(_config.AppearanceScale, _config.AppearanceDuration).SetEase(_config.AppearanceEase))
                .Append(slave.DOPunchScale(_config.PunchScale, _config.PunchDuration, _config.Vibrato, _config.Elasticy)
                    .SetEase(_config.PunchEase));
            _seq.Play();
        }

        public void Dispose()
        {
            _seq?.Kill();
            _seq = null;
        }
    }
}