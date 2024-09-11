using System;
using DG.Tweening;
using UnityEngine;

namespace ATG.Transition
{
    public class HideVerticalUITransition : ITransitionBehaviour<RectTransform>
    {
        private readonly HideVerticalUITransitionData _config;
        private readonly float _screenHeight;

        private Tween _tween;

        public HideVerticalUITransition(HideVerticalUITransitionData config)
        {
            _config = config;
            _screenHeight = Screen.height;
        }

        public void Execute(RectTransform slave, Action callback = null)
        {
            Dispose();

            Vector2 endPos = slave.anchoredPosition;
            endPos.y = -_screenHeight;

            _tween = slave.DOAnchorPos(endPos, _config.Duration)
                .SetEase(_config.Ease)
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