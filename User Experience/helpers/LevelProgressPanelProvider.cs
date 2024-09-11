using System;
using ATG.DTO;
using ATG.Level;
using ATG.Observable;
using UnityEngine;

namespace ATG.UI
{
    [Serializable]
    public sealed class LevelProgressPanelContainer
    {
        [SerializeField] private InfoPanel currentLevel;
        [SerializeField] private InfoPanel nextLevel;
        [SerializeField] private InfoPanel nearBootLevel;
        [Space(15)]
        [SerializeField] private ProgressBar levelProgressBar;

        public LevelProgressPanelProvider Create(ILevelProgressable levelProgres)
        {
            return new LevelProgressPanelProvider
            (
                levelProgres,
                currentLevel,
                nextLevel,
                nearBootLevel,
                levelProgressBar
            );
        }
    }

    public sealed class LevelProgressPanelProvider : IDisposable
    {
        private const string InfiniteSymbol = "\u221E";

        private ILevelProgressable _levelProgress;

        private readonly InfoPanel _currentLevel;
        private readonly InfoPanel _nextLevel;
        private readonly InfoPanel _nearBoostLevel;

        private readonly ProgressBar _levelProgressBar;

        private ObserveDisposable _dis;

        public LevelProgressPanelProvider(ILevelProgressable levelService, 
            InfoPanel currentLvl, InfoPanel nextLvl, InfoPanel nearBoostLvl,
            ProgressBar lvlProgressBar)
        {
            _levelProgress = levelService;

            _currentLevel = currentLvl;
            _nextLevel = nextLvl;
            _nearBoostLevel = nearBoostLvl;

            _levelProgressBar = lvlProgressBar;
        }

        private int _previousLevel = 0;

        public void Execute()
        {
            UpdateLevelProgress(_levelProgress.GetCurrentLevelProgress());
            
            _dis = _levelProgress.Experience
                .Subscribe(_ => UpdateLevelProgress(_levelProgress.GetCurrentLevelProgress()));
        }

        public void Dispose()
        {
           _dis?.Dispose();
           _dis = null;

           _levelProgressBar.Hide();
        }

        private void UpdateLevelProgress(in LevelProgressDTO data)
        {
            int curLevelValue = data.CurrentLevel;

            _currentLevel?.Show(this, curLevelValue.ToString());
            _levelProgressBar.Show(this, null);

            UpdateNextLevelData(data.IsCurrentLevelFinal == false, data.NextLevel);
            UpdateNearBoostData(data.HasNextBoostLevel, data.NextBoostLevel);
            UpdateProgressBar(data.CurrentExperience, data.ExperienceToNextLevel, curLevelValue != _previousLevel &&
                                            data.IsCurrentLevelFinal == false);

            _previousLevel = curLevelValue;
        }

        private void UpdateNextLevelData(in bool hasValue, in int value)
        {
            _nextLevel.Show(this, hasValue ? value.ToString() : InfiniteSymbol);
        }

        private void UpdateNearBoostData(in bool hasValue, in int value)
        {
            _nearBoostLevel.Show(this, hasValue ? value.ToString() : InfiniteSymbol);
        }

        private void UpdateProgressBar(in int currentExperience, in int needExperience, bool isResetBefore)
        {
            if (isResetBefore == true) _levelProgressBar.ResetProgress();
            _levelProgressBar.SetProgressWithProgressOutput(currentExperience, needExperience);
        }
    }
}