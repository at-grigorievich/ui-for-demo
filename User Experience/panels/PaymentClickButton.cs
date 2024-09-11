using System;
using ATG.DTO;
using ATG.Product;
using ATG.Wallet;
using TMPro;
using UnityEngine;
using VContainer;

namespace ATG.UI
{
    [Serializable]
    public sealed class PaymentClickButtonContainer
    {
        [SerializeField] private ProductType payProductType;
        [Space(5)]
        [SerializeField] private ClickButton button;
        [SerializeField] private TextMeshProUGUI priceOutput;
        [SerializeField] private ColoredSkinSwitcher skinSwitcher;

        public PaymentClickButton Create()
        {
            if (UserInterfaceService.DIResolver.TryResolve(out IPaymentsService paymentsService) == false)
            {
                throw new VContainerException(typeof(IPaymentsService), "Can't resolve Payment Service !");
            }

            if (UserInterfaceService.DIResolver.TryResolve(out ICurrencyService currencyService) == false)
            {
                throw new VContainerException(typeof(IPaymentsService), "Can't resolve Currency Service !");
            }

            return new PaymentClickButton
            (
                currencyService,
                paymentsService,
                paymentsService.GetProductInfoByType(payProductType),
                button,
                skinSwitcher,
                priceOutput
            );
        }
    }

    public sealed class PaymentClickButton : IDisposable
    {
        private readonly ClickButton _button;
        private readonly ColoredSkinSwitcher _skinSwitcher;
        private readonly TextMeshProUGUI _priceOutput;

        private readonly IPaymentsService _paymentService;
        private readonly ICurrencyService _currencyService;
        private readonly ProductDTO _payProduct;

        private IDisposable _dis;

        public PaymentClickButton(ICurrencyService currencyService, IPaymentsService paymentsService, ProductDTO product,
            ClickButton btn, ColoredSkinSwitcher skinSwitcher, TextMeshProUGUI priceOutput)
        {
            _currencyService = currencyService;
            _paymentService = paymentsService;

            _payProduct = product;

            _button = btn;
            _skinSwitcher = skinSwitcher;
            _priceOutput = priceOutput;
        }

        public void Execute(object sender, Action callback)
        {
            _dis = _currencyService.Currency.Subscribe(CheckBuyAllow);
            CheckBuyAllow(_currencyService.Currency.Value);

            _priceOutput.text = _payProduct.Cost.ToString();

            Action clickCallback = OnButtonClick;
            clickCallback += callback;

            _button.Show(sender, clickCallback);
        }

        public void Dispose()
        {
            _dis?.Dispose();
            _dis = null;

            _button.Hide();
        }

        private void CheckBuyAllow(int currentPlayerCurrency)
        {
            bool isAvailableCost = currentPlayerCurrency >= _payProduct.Cost;

            SkinType skin = isAvailableCost ? SkinType.First : SkinType.Second;
            _skinSwitcher.SwitchToSkin(skin);

            _button.SetInteractable(isAvailableCost);
        }

        private void OnButtonClick()
        {
            switch(_paymentService.TryUseProductByType(_payProduct.Type))
            {
                case ProductOrderStatus.None:
                    throw new Exception("may be Payment Service is disactive");
                case ProductOrderStatus.NotAvailableProduct:
                    throw new Exception($"Payment Service doesnt contains {_payProduct.Type}");
                case ProductOrderStatus.NotYetCurrency:
                    #if UNITY_EDITOR
                        throw new Exception("Not enough currency");
                    #endif
            }
        }
    }
}