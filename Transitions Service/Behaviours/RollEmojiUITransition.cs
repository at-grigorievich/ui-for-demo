using System;
using DG.Tweening;
using UnityEngine;

namespace ATG.Transition
{
    public sealed class RollEmojiUITransition : ITransitionBehaviour<RectTransform>
    {
        private readonly RollEmojiUITransitionData _config;

        private Tween _tween;

        public RollEmojiUITransition(RollEmojiUITransitionData config)
        {
            _config = config;
        }

        public void Execute(RectTransform slave, Action callback = null)
        {
            Dispose();

            Vector2 startPos = slave.anchoredPosition;
            Vector2 endPos = startPos;

            startPos.y = slave.sizeDelta.y;
            endPos.y = -slave.sizeDelta.y;

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