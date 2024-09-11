using System;
using ATG.DTO;
using ATG.Level;
using ATG.Observable;
using UnityEngine;

namespace ATG.UI
{
    [Serializable]
    public sealed class LostProgressPanelContainer
    {
        [SerializeField] private InfoPanel currentLevel;
        [SerializeField] private InfoPanel boostLevel;
        [Space(15)]
        [SerializeField] private ProgressBar levelProgressBar;

        public LostProgressPanelProvider Create(ILevelProgressable levelProgress)
        {
            return new LostProgressPanelProvider
            (
                levelProgress,
                currentLevel,
                boostLevel,
                levelProgressBar
            );
        }
    }

    public sealed class LostProgressPanelProvider : IDisposable
    {
        private const string InfiniteSymbol = "\u221E";

        private ILevelProgressable _levelProgress;

        private readonly InfoPanel _currentLevel;
        private readonly InfoPanel _nearBoostLevel;

        private readonly ProgressBar _levelProgressBar;

        public LostProgressPanelProvider(ILevelProgressable levelService,
            InfoPanel currentLvl, InfoPanel nearBoostLvl, ProgressBar lvlProgressBar)
        {
            _levelProgress = levelService;

            _currentLevel = currentLvl;
            _nearBoostLevel = nearBoostLvl;

            _levelProgressBar = lvlProgressBar;
        }

        public void Execute()
        {
            ShowLevelProgress(_levelProgress.GetCurrentLevelProgress());
        }

        public void Dispose()
        {
            _levelProgressBar.Hide();
        }

        private void ShowLevelProgress(in LevelProgressDTO data)
        {
            int curLevelValue = data.CurrentLevel;

            _currentLevel?.Show(this, curLevelValue.ToString());
            _levelProgressBar.Show(this, null);

            ShowNearBoostData(data.HasNextBoostLevel, data.NextBoostLevel);

            if(data.HasNextBoostLevel == true)
            {
                ShowProgressBar(data.CurrentExperience, data.ExperienceToBoostLevel);
            }
            else
            {
                ShowProgressBar(data.CurrentExperience, data.CurrentExperience);
            }
        }

        private void ShowNearBoostData(in bool hasValue, in int value)
        {
            _nearBoostLevel.Show(this, hasValue ? value.ToString() : InfiniteSymbol);
        }

        private void ShowProgressBar(in int currentExperience, in int needExperience)
        {
            _levelProgressBar.SetProgressWithProgressOutput(currentExperience, needExperience);
        }
    }
}