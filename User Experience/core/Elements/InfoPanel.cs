using System;
using ATG.Transition;
using TMPro;
using UnityEngine;

namespace ATG.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class InfoPanel : UIElement
    {
        [SerializeField] private TextMeshProUGUI infoOutput;
        [SerializeField] private bool useAnimation;
        [SerializeField] private bool ignoreEquals;

        private ITransitionBehaviour<RectTransform> _changeOutputTransition;

        private CanvasGroup _canvasGroup;

        protected void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();

            if (useAnimation == true)
            {
                if (UserInterfaceService.DIResolver.TryResolve(out ITransitionService transitionService) == true)
                {
                    _changeOutputTransition = transitionService
                                        .GetTransitionBehaviourByType<RectTransform>(TransitionType.ChangeTextOutput);
                }
            }
        }

        public override void Show(object sender, object data)
        {
            if (data is not string dataStr) throw new ArgumentException("data is not string!");

            base.Show(sender, data);

            if(ignoreEquals == true)
            {
                if(dataStr == infoOutput.text) return;
            }

            infoOutput.text = dataStr;

            Animate();
        }

        public override void Hide()
        {
            base.Hide();

            if (useAnimation == true)
            {
                _changeOutputTransition?.Dispose();
            }
        }

        public override void SetActive(bool isActive)
        {
            base.SetActive(isActive);
            _canvasGroup.alpha = isActive ? 1f : 0f;
        }
    
        protected void Animate()
        {
            if (useAnimation == true)
            {
                _changeOutputTransition.Execute(infoOutput.rectTransform);
            }
        }
    }
}