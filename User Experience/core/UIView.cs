using ATG.Transition;
using UnityEngine;
using VContainer;

using ShowTranstion = ATG.Transition.ITransitionBehaviour<UnityEngine.RectTransform>;
using HideTransition = ATG.Transition.ITransitionBehaviour<UnityEngine.RectTransform>;
using System;

namespace ATG.UI
{
    [RequireComponent(typeof(Canvas))]
    public abstract class UIView : UIElement
    {
        [SerializeField] protected RectTransform wrapper;

        private Canvas _canvas;

        protected ITransitionService _transitionService;

        private ShowTranstion _showTransition;
        private HideTransition _hideTransition;

        public abstract UIElementType ViewType { get; }

        protected void Awake()
        {
            _canvas = GetComponent<Canvas>();

            if (UserInterfaceService.DIResolver.TryResolve(out _transitionService) == false)
            {
                throw new VContainerException(typeof(ITransitionService), "Can't resolve Transition Service !");
            }

            InitTransitions();
        }

        public override void SetActive(bool isActive)
        {
            base.SetActive(isActive);

            _canvas.enabled = isActive;
        }

        public void ChangeLayerIndex(int index) => _canvas.sortingOrder = index;

        protected virtual void InitTransitions()
        {
            _showTransition = _transitionService.GetTransitionBehaviourByType<RectTransform>(TransitionType.ShowVerticalUI);
            _hideTransition = _transitionService.GetTransitionBehaviourByType<RectTransform>(TransitionType.HideVerticalUI);
        }

        protected virtual void ShowTransition(Action callback) =>
            _showTransition.Execute(wrapper, callback);
        
        protected virtual void HideTransition(Action callback) =>
            _hideTransition.Execute(wrapper, callback);

        protected virtual void DisposeTransitions() 
        {
            _showTransition.Dispose();
            _hideTransition.Dispose();
        }
    }
}