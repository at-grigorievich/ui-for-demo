using System;
using ATG.DTO;
using ATG.Transition;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace ATG.UI
{
    public class ScaleClickButton: UIElement, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform total;
        [Space(5)]
        [SerializeField] private Vector3 defaultScale;
        [SerializeField] private Vector3 selectScale;

        protected ITransitionService _transitionService;
        protected ITransitionBehaviour<ScaleButtonDTO> _transition;

        private bool _isInteractable;

        private Action _onClick;

        protected void Awake()
        {
            if (UserInterfaceService.DIResolver.TryResolve(out _transitionService) == false)
            {
                throw new VContainerException(typeof(ITransitionService), "Can't resolve ICurrencyService Service !");
            }

            GetTransitions();
        }

        public override void Show(object sender, object data)
        {
            base.Show(sender, data);
            if(data is Action callback)
            {
                _onClick = callback;
            }

            _isInteractable = true;
        }

        public override void Hide()
        {
            base.Hide();
            
            _onClick = null;

            _isInteractable = false;

            _transition.Execute(new ScaleButtonDTO(total, defaultScale), null);
        }

        public void SetInteractable(bool isInteractable) => _isInteractable = isInteractable;

        public void OnPointerDown(PointerEventData eventData)
        {
            if(_isInteractable == false) return;

            _transition.Execute(new ScaleButtonDTO(total, selectScale), null);
            _onClick?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _transition.Execute(new ScaleButtonDTO(total, defaultScale), null);
        }

        protected virtual void GetTransitions()
        {
            _transition = _transitionService.GetTransitionBehaviourByType<ScaleButtonDTO>(TransitionType.ScaleButton);
        }
    }
}