using System;
using ATG.Level;
using UnityEngine;
using VContainer;

namespace ATG.UI
{
    public sealed class GameView : UIView
    {
        [SerializeField] private GoldInfoPanel goldOutput;
        [SerializeField] private ScaleClickButton shopMainButton;
        [Space(15)]
        [SerializeField] private LevelProgressPanelContainer levelProgressContainer; 
        [Space(15)]
        [SerializeField] private PaymentClickButtonContainer paymentButtonContainer;

        private LevelProgressPanelProvider _lvlProgressPanelProvider;
        private PaymentClickButton _paymentClickButton;
        
        private IUserInterfaceService _uiService;

        public override UIElementType ViewType => UIElementType.GameView;

        private new void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);

            if(UserInterfaceService.DIResolver.TryResolve(out ILevelProgressable levelProgress) == false)
            {
                throw new VContainerException(typeof(ILevelProgressable), "Can't resolve Level Progress Service !");
            }

            if(UserInterfaceService.DIResolver.TryResolve(out _uiService) == false)
            {
                throw new VContainerException(typeof(IUserInterfaceService), "Can't resolve UI Service !");
            }

            _lvlProgressPanelProvider = levelProgressContainer.Create(levelProgress);
            _paymentClickButton = paymentButtonContainer.Create();
        }

        public override void Show(object sender, object data)
        {
            base.Show(sender, data);

            goldOutput.Show(this, null);

            _paymentClickButton.Execute(this, null);
            _lvlProgressPanelProvider.Execute();

            Action openShopView = OpenShopView;
            shopMainButton.Show(this,openShopView);
        }
        
        public override void Hide()
        {
            base.Hide();

            goldOutput.Hide();      
            shopMainButton.Hide();

            _lvlProgressPanelProvider.Dispose();
            _paymentClickButton.Dispose();
        }

        private void OpenShopView()
        {
            _uiService.ShowViewByTypeAsync(UIElementType.ShowView, this, null, true);
        }
    }
}