using ATG.Transition;
using VContainer;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using ATG.DTO;

namespace ATG.UI
{
    public class ClickButton: UIElement, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform mainRect;
        [SerializeField] private RectTransform backgroundRect;
        [SerializeField] private RectTransform contentRect;

        protected ITransitionService _transitionService;
        protected ITransitionBehaviour<GraphicButtonDTO> _transition;

        private bool _isInteractable;

        private Action _onClick;

        protected void Awake()
        {
            if (UserInterfaceService.DIResolver.TryResolve(out _transitionService) == false)
            {
                throw new VContainerException(typeof(ITransitionService), "Can't resolve ITransitionService !");
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
            _transition.Dispose();
            _onClick = null;

            _isInteractable = false;
        }

        public void SetInteractable(bool isInteractable) => _isInteractable = isInteractable;

        public void OnPointerDown(PointerEventData eventData)
        {
            if(_isInteractable == false) return;

            _transition.Execute(new GraphicButtonDTO(mainRect, backgroundRect, contentRect, true), null);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if(_isInteractable == false) return;

            _transition.Execute(new GraphicButtonDTO(mainRect, backgroundRect, contentRect, false), null);
            _onClick?.Invoke();
        }

        protected virtual void GetTransitions()
        {
            _transition = _transitionService.GetTransitionBehaviourByType<GraphicButtonDTO>(TransitionType.PressButton);
        }
    }
}