using System;
using ATG.Ad;
using ATG.Product;
using UnityEngine;
using VContainer;

namespace ATG.UI
{
    [Serializable]
    public class ProgressAdClickButtonContainer
    {
        [SerializeField] private AdType adType;
        [SerializeField] private ProductType productType;

        [SerializeField] private ClickButton clickButton;
        [SerializeField] private ColoredSkinSwitcher buttonSkinSwitcher;
        [Space(10)]
        [SerializeField] private ProgressBar progressBar;
        [SerializeField, Range(0, 25)] private int needAdWatchedCount;

        public ProgressAdClickButton Create(IPaymentsService paymentsService, IAdvertismentService adService)
        {
            return new ProgressAdClickButton
            (
                clickButton,
                progressBar,
                needAdWatchedCount,
                buttonSkinSwitcher,
                paymentsService,
                adService,
                adType,
                productType
            );
        }
    }

    public sealed class ProgressAdClickButton : AdClickButton
    {
        private readonly ProgressBar _progressBar;

        private readonly int _needAdWatchedCount;
        private int _currentAdWatchedCount;

        public ProgressAdClickButton(ClickButton btn, ProgressBar progressBar, int needAdWatched,
             ColoredSkinSwitcher btnSkin, IPaymentsService paymentsService, IAdvertismentService adService,
            AdType adType, ProductType productType) :
            base(btn, btnSkin, paymentsService, adService, adType, productType)
        {
            _progressBar = progressBar;

            _needAdWatchedCount = needAdWatched;
        }

        public override void Execute(object sender, Action callback)
        {
            base.Execute(sender, callback);
            _progressBar?.Show(this, null);

            _progressBar?.SetProgressWithProgressOutput(_currentAdWatchedCount, _needAdWatchedCount);
        }

        protected override void GetReward()
        {
            _currentAdWatchedCount++;

            _progressBar?.SetProgressWithProgressOutput(_currentAdWatchedCount, _needAdWatchedCount);

            if (_currentAdWatchedCount >= _needAdWatchedCount)
            {
                base.GetReward();
                Reset();
            }
        }

        private void Reset()
        {
            _currentAdWatchedCount = 0;
            _progressBar?.ResetProgressWithProgressOutput(_needAdWatchedCount);
        }
    }
}