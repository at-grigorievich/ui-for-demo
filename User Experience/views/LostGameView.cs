using System;
using ATG.Level;
using ATG.Observable;
using ATG.Transition;
using UnityEngine;
using VContainer;

using RollTransition = ATG.Transition.ITransitionBehaviour<UnityEngine.RectTransform>;

namespace ATG.UI
{
    public sealed class LostGameView : UIView
    {
        [SerializeField] private GoldInfoPanel goldOutput;
        [Space(15)]
        [SerializeField] private LostProgressPanelContainer lostProgressContainer;
        [SerializeField] private PaymentClickButtonContainer continueButtonContainer;
        [SerializeField] private ClickButton restartButton;
        [Space(10)]
        [SerializeField] private RectTransform emojiRect;

        private IMessageBroker _messageBroker;

        private LostProgressPanelProvider _lostProgressProvider;
        private PaymentClickButton _continueClickButton;

        private RollTransition _rollTransition;

        public override UIElementType ViewType => UIElementType.LostGameView;

        private new void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);

            if (UserInterfaceService.DIResolver.TryResolve(out ILevelProgressable levelProgress) == false)
            {
                throw new VContainerException(typeof(ILevelProgressable), "Can't resolve Level Progress Service !");
            }

            if (UserInterfaceService.DIResolver.TryResolve(out _messageBroker) == false)
            {
                throw new VContainerException(typeof(IMessageBroker), "Can't resolve IMessageBroker !");
            }

            _lostProgressProvider = lostProgressContainer.Create(levelProgress);
            _continueClickButton = continueButtonContainer.Create();

            restartButton.SetInteractable(true);
        }

        public override void Show(object sender, object data)
        {
            base.Show(sender, data);

            emojiRect.gameObject.SetActive(false);

            void Execute()
            {
                goldOutput.Show(this, null);

                _continueClickButton.Execute(this, OnContinueClick);
                restartButton.Show(this, (Action)OnRestartClick);

                _lostProgressProvider.Execute();

                emojiRect.gameObject.SetActive(true);
                _rollTransition.Execute(emojiRect);
            }

            DisposeTransitions();
            ShowTransition(Execute);
        }

        public override void Hide()
        {
            void Execute()
            {
                base.Hide();

                goldOutput.Hide();

                _continueClickButton.Dispose();
                _lostProgressProvider.Dispose();

                restartButton.Hide();
            }

            _rollTransition.Dispose();
            DisposeTransitions();
            HideTransition(Execute);
        }

        protected override void InitTransitions()
        {
            base.InitTransitions();
            _rollTransition = _transitionService.GetTransitionBehaviourByType<RectTransform>(TransitionType.RollEmojiUI);
        }

        private void OnContinueClick()
        {
            Hide();
        }

        private void OnRestartClick()
        {
            Hide();
            _messageBroker.Send(new RestartLevelMessage());
        }
    }
}