using System;
using ATG.DTO;
using ATG.Level;
using ATG.Transition;
using UnityEngine;
using VContainer;

using RollTransition = ATG.Transition.ITransitionBehaviour<UnityEngine.RectTransform>;

namespace ATG.UI
{
    public sealed class CompleteGameView : UIView
    {
        [SerializeField] private GoldInfoPanel goldOutput;
        [SerializeField] private InfoPanel nextTargetLevel;
        [Space(10)]
        [SerializeField] private ClickButton continueButton;
        [SerializeField] private AdClickButtonContainer adButtonContainer;
        [Space(10)]
        [SerializeField] private RectTransform emojiRect;

        private RollTransition _rollTransition;

        private AdClickButton _adButton;

        private ILevelProgressable _levelService;

        private Action _completeBtnClick;

        public override UIElementType ViewType => UIElementType.CompleteGameView;

        private new void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);

            if (UserInterfaceService.DIResolver.TryResolve(out _levelService) == false)
            {
                throw new VContainerException(typeof(ILevelProgressable), "Can't resolve Level Progress Service !");
            }

            _adButton = adButtonContainer.Create();

            continueButton.SetInteractable(true);
        }

        public override void Show(object sender, object data)
        {
            base.Show(sender, data);

            emojiRect.gameObject.SetActive(false);
            nextTargetLevel.Show(this, string.Empty);

            void execute()
            {
                goldOutput.Show(this, null);

                if (data is Action continueClick)
                {
                    _completeBtnClick = continueClick;
                }

                ShowNextBoostLevel();

                continueButton.Show(this, (Action)OnContinuePressed);
                _adButton.Execute(this, null);

                emojiRect.gameObject.SetActive(true);
                _rollTransition.Execute(emojiRect);
            }

            DisposeTransitions();
            ShowTransition(execute);
        }

        public override void Hide()
        {
            _rollTransition?.Dispose();

            void execute()
            {
                base.Hide();

                continueButton.Hide();
                _adButton.Dispose();

                goldOutput.Hide();
            }


            DisposeTransitions();
            HideTransition(execute);
        }

        protected override void InitTransitions()
        {
            base.InitTransitions();
            _rollTransition = _transitionService.GetTransitionBehaviourByType<RectTransform>(TransitionType.RollEmojiUI);
        }

        private void OnContinuePressed()
        {
            Hide();
            _completeBtnClick?.Invoke();
        }

        private void ShowNextBoostLevel()
        {
            LevelProgressDTO data = _levelService.GetCurrentLevelProgress();

            if (data.HasNextBoostLevel == true)
            {
                nextTargetLevel.Show(this, data.NextBoostLevel.ToString());
            }
            else nextTargetLevel.Show(this, "no");
        }
    
        [ContextMenu("Test roll emoji")]
        public void TestRollEmoji() => _rollTransition.Execute(emojiRect);
    }
}