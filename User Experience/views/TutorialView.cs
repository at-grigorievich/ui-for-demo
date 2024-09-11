using ATG.DTO;
using ATG.Transition;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using HandTransition = ATG.Transition.ITransitionBehaviour<ATG.DTO.HandTutorialDTO>;

namespace ATG.UI
{
    public sealed class TutorialView : UIView
    {
        [SerializeField] private Image background;
        [SerializeField] private CanvasGroup infoGroup;
        [SerializeField] private CanvasGroup handGroup;
        [Space(15)]
        [SerializeField] private TextMeshProUGUI tutorialText;
        [SerializeField] private ClickButton clickButton;

        private HandTransition _handTransition;

        public override UIElementType ViewType => UIElementType.TutorialView;

        private new void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        public override void Show(object sender, object data)
        {
            base.Show(sender, data);

            if (data is TextTutorialDTO textTutorialDTO)
            {
                infoGroup.alpha = 1f;
                handGroup.alpha = 0f;

                background.enabled = true;

                tutorialText.text = textTutorialDTO.TutorialPhrase;
                clickButton.Show(this, textTutorialDTO.OnLearned);
            }
            else if (data is HandTutorialDTO handTutorialDTO)
            {
                clickButton.Hide();

                infoGroup.alpha = 0f;
                handGroup.alpha = 1f;

                background.enabled = false;

                _handTransition.Execute(new HandTutorialDTO(handGroup.GetComponent<RectTransform>(), handTutorialDTO));
            }
        }

        public override void Hide()
        {
            base.Hide();
            clickButton.Hide();

            DisposeTransitions();
        }

        protected override void InitTransitions()
        {
            base.InitTransitions();
            _handTransition = _transitionService.GetTransitionBehaviourByType<HandTutorialDTO>(TransitionType.TutorialHandMove);
        }

        protected override void DisposeTransitions()
        {
            base.DisposeTransitions();
            _handTransition.Dispose();
        }
    }
}