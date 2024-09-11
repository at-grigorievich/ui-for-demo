using System;
using ATG.Ad;
using ATG.Product;
using UnityEngine;
using VContainer;

namespace ATG.UI
{
    public sealed class ShopView : UIView
    {
        [SerializeField] private GoldInfoPanel currencyInfo;
        [SerializeField] private ClickButton closeButton;
        [Space(10)]
        [SerializeField] private ProgressAdClickButtonContainer[] adButtonContainers;

        private ProgressAdClickButton[] _adButtons;

        public override UIElementType ViewType => UIElementType.ShowView;

        private new void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);

            if (UserInterfaceService.DIResolver.TryResolve(out IPaymentsService paymentsService) == false)
            {
                throw new VContainerException(typeof(IPaymentsService), "Can't resolve Payment Service !");
            }
            if (UserInterfaceService.DIResolver.TryResolve(out IAdvertismentService adService) == false)
            {
                throw new VContainerException(typeof(IAdvertismentService), "Can't resolve Advertisment Service !");
            }

            _adButtons = new ProgressAdClickButton[adButtonContainers.Length];

            for (int i = 0; i < adButtonContainers.Length; i++)
            {
                _adButtons[i] = adButtonContainers[i].Create(paymentsService, adService);
            }
        }

        public override void Show(object sender, object data)
        {
            base.Show(sender, data);

            void Execute()
            {
                Action hideAction = Hide;
                closeButton.Show(this,  hideAction);
            }

            currencyInfo.Show(this, null);

            foreach(var adBtn in _adButtons)
            {
                adBtn.Execute(this, null);
            }

            DisposeTransitions();
            ShowTransition(Execute);
        }

        public override void Hide()
        {
            void Execute()
            {
                base.Hide();

                currencyInfo.Hide();
                closeButton.Hide();

                foreach(var adBtn in _adButtons)
                {
                    adBtn.Dispose();
                }
            }

            DisposeTransitions();
            HideTransition(Execute);
        }

    }
}