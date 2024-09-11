using System;
using DG.Tweening;
using UnityEngine;

namespace ATG.Transition
{
    public class ShowVerticalUITransition : ITransitionBehaviour<RectTransform>
    {
        private readonly ShowVerticalUITransitionData _config;
        private readonly float _screenHeight;


        private Tween _tween;

        public ShowVerticalUITransition(ShowVerticalUITransitionData config)
        {
            _config = config;
            _screenHeight = Screen.height;
        }

        public void Execute(RectTransform slave, Action callback = null)
        {
            Dispose();

            Vector2 startPos = slave.anchoredPosition;
            Vector2 endPos = Vector2.zero;

            startPos.y = _screenHeight;
        
            slave.anchoredPosition = startPos;

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