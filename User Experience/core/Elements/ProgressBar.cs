using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace ATG.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ProgressBar : UIElement
    {
        [SerializeField] private RectTransform progressRect;
        [SerializeField] private float fillProgressWidth = 100f;
        [Space(15)]
        [SerializeField] private TextMeshProUGUI output;
        [Space(15)]
        [SerializeField] private bool withAnimation;
        [SerializeField] private ProgressBarTween animationConfig;

        private Tween _animate;

        private CanvasGroup _canvasGroup;

        protected void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public override void SetActive(bool isActive)
        {
            base.SetActive(isActive);
            _canvasGroup.alpha = isActive ? 1f : 0f;

            _animate?.Kill();
            _animate = null;
        }

        public void ResetProgress() => UpdateProgress(0f);

        /// <param name="progressRate">progress between 0f & 1f</param>
        public void SetProgress(float progressRate)
        {
            float nextWidth = progressRate * fillProgressWidth;

            if (withAnimation == true)
            {
                UpdateProgressWithAnimation(nextWidth);
            }
            else
            {
                UpdateProgress(nextWidth);
            }
        }
        public void SetProgressWithProgressOutput(float current, float max)
        {
            float progressRate = current / max;
            SetProgress(progressRate);

            if (output == null) return;
            output.text = $"{current} / {max}";
        }

        public void ResetProgressWithProgressOutput(float max)
        {
            if (output == null) return;
            output.text = $"{0} / {max}";

            _animate?.Kill();
            _animate = null;
            UpdateProgress(0f);
        }

        private void UpdateProgress(float nextWidth)
        {
            Vector2 nextSizeDelta = progressRect.sizeDelta;
            nextSizeDelta.x = nextWidth;

            progressRect.sizeDelta = nextSizeDelta;
        }
        private void UpdateProgressWithAnimation(float nextWidth)
        {
            _animate?.Kill();
            _animate = animationConfig.Animate(progressRect, nextWidth);
            _animate.Play();
        }

        [Serializable]
        private sealed class ProgressBarTween
        {
            [SerializeField] private float duration = 0.25f;
            [SerializeField] private Ease ease = Ease.Linear;

            public Tween Animate(RectTransform rect, float targetWidth)
            {
                Vector2 nextSizeDelta = rect.sizeDelta;
                nextSizeDelta.x = targetWidth;

                return rect.DOSizeDelta(nextSizeDelta, duration).SetEase(ease);
            }
        }
    }
}