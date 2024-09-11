using System;
using ATG.Ad;
using ATG.Observable;
using ATG.Product;
using UnityEngine;
using VContainer;

namespace ATG.UI
{
    [Serializable]
    public class AdClickButtonContainer
    {
        [SerializeField] private AdType adType;
        [SerializeField] private ProductType productType;

        [SerializeField] private ClickButton clickButton;
        [SerializeField] private ColoredSkinSwitcher buttonSkinSwitcher;

        public AdClickButton Create()
        {
            if (UserInterfaceService.DIResolver.TryResolve(out IPaymentsService paymentsService) == false)
            {
                throw new VContainerException(typeof(IPaymentsService), "Can't resolve Payment Service !");
            }
            if (UserInterfaceService.DIResolver.TryResolve(out IAdvertismentService adService) == false)
            {
                throw new VContainerException(typeof(IAdvertismentService), "Can't resolve Advertisment Service !");
            }

            return new AdClickButton
            (
                clickButton,
                buttonSkinSwitcher,
                paymentsService,
                adService,
                adType,
                productType
            );
        }
    }

    public class AdClickButton : IDisposable
    {
        private readonly ClickButton _clickButton;
        private readonly ColoredSkinSwitcher _buttonSkin;

        protected readonly IPaymentsService _paymentService;
        protected readonly IAdvertismentService _adService;

        protected readonly AdType _adType;
        protected readonly ProductType _productType;

        private ObserveDisposable _dis;

        private Action _lastClickCallback;

        public AdClickButton(ClickButton btn, ColoredSkinSwitcher btnSkin,
            IPaymentsService paymentsService, IAdvertismentService adService,
            AdType adType, ProductType productType)
        {
            _clickButton = btn;
            _buttonSkin = btnSkin;

            _paymentService = paymentsService;
            _adService = adService;

            _adType = adType;
            _productType = productType;
        }

        public virtual void Execute(object sender, Action callback)
        {
            var adVar = _adService[_adType];

            _dis = adVar.Subscribe(UpdateBtnInteractable);
            UpdateBtnInteractable(adVar.Value);

            _lastClickCallback = callback;
            _clickButton.Show(sender, (Action)OnButtonClick);
        }


        public void Dispose()
        {
            _dis?.Dispose();

            _clickButton.Hide();

            _lastClickCallback = null;
        }

        private void UpdateBtnInteractable(bool isAvailable)
        {
            SkinType skin = isAvailable ? SkinType.First : SkinType.Second;
            _buttonSkin.SwitchToSkin(skin);

            _clickButton.SetInteractable(isAvailable);
        }

        private void OnButtonClick()
        {
            _adService.ShowByType(_adType, OnAdCompleted);
        }

        private void OnAdCompleted(bool isCompleted)
        {
            if (_adType == AdType.Rewarded && isCompleted == true)
            {
                GetReward();
            }

            _lastClickCallback?.Invoke();
        }

        protected virtual void GetReward()
        {
            _paymentService.UseProductByType(_productType);
        }
    }
}