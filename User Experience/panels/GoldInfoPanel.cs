using System;
using ATG.Observable;
using ATG.Wallet;
using UnityEngine;
using VContainer;

namespace ATG.UI
{
    public sealed class GoldInfoPanel : InfoPanel
    {
        [SerializeField] private ScaleClickButton showShopButton;

        private ICurrencyService _currencyService;
        private IUserInterfaceService _uiService;

        private ObserveDisposable _dis;

        private new void Awake()
        {
            base.Awake();

            if (UserInterfaceService.DIResolver.TryResolve(out _currencyService) == false)
            {
                throw new VContainerException(typeof(ICurrencyService), "Can't resolve ICurrencyService Service !");
            }

            if (UserInterfaceService.DIResolver.TryResolve(out _uiService) == false)
            {
                throw new VContainerException(typeof(IUserInterfaceService), "Can't resolve IUserInterface Service !");
            }
        }

        public override void Show(object sender, object data)
        {
            base.Show(sender, _currencyService.Currency.Value.ToString());
            _dis = _currencyService.Currency.Subscribe(CurrencyChanged);

            if (showShopButton != null)
            {
                Action onClick = OpenShopView;
                showShopButton.Show(this, onClick);
            }
        }

        public override void Hide()
        {
            base.Hide();
            _dis?.Dispose();

            showShopButton?.Hide();
        }

        private void CurrencyChanged(int nextCurrency)
        {
            base.Show(this, nextCurrency.ToString());
        }

        private void OpenShopView()
        {
            _uiService.ShowViewByTypeAsync(UIElementType.ShowView, this, null, true);
        }
    }
}