using System;
using ATG.DTO;
using DG.Tweening;

namespace ATG.Transition
{
    public sealed class ScaleButtonTransition : ITransitionBehaviour<ScaleButtonDTO>
    {
        private readonly ScaleButtonTransitionData _config;

        private Tween _tween;

        public ScaleButtonTransition(ScaleButtonTransitionData config)
        {
            _config = config;
        }
        
        public void Execute(ScaleButtonDTO slave, Action callback = null)
        {
            Dispose();

            _tween = slave.ButtonRect.DOScale(slave.Scale, _config.Duration)
                .OnComplete(() => callback?.Invoke());

            _tween.Play();
        }

        public void Dispose()
        {
            _tween?.Kill();
            _tween = null;
        }
    }
}