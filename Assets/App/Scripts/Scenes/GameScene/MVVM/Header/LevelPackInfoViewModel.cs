using App.Scripts.Scenes.GameScene.Features.Levels.LevelProgress;
using App.Scripts.Scenes.GameScene.Features.Levels.LevelView;
using App.Scripts.Scenes.GameScene.Features.ScoreAnimation;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.MVVM.Header
{
    public class LevelPackInfoViewModel
    {
        private readonly ILevelPackBackgroundView _levelPackBackgroundView;
        private readonly ILevelPackInfoView _levelPackInfoView;
        private readonly IScoreAnimationService _scoreAnimationService;

        public LevelPackInfoViewModel(
            ILevelPackBackgroundView levelPackBackgroundView,
            ILevelPackInfoView levelPackInfoView,
            IScoreAnimationService scoreAnimationService)
        {
            _levelPackBackgroundView = levelPackBackgroundView;
            _levelPackInfoView = levelPackInfoView;
            _scoreAnimationService = scoreAnimationService;
        }

        public void UpdateView(LevelPackInfoRecord info)
        {
            int currentLevel = info.CurrentLevelIndex + 1;
            int allLevelsCount = info.AllLevelsCountFromPack;
            
            string levelPassText = $"{currentLevel.ToString()}/{allLevelsCount.ToString()}";
            string progressText = $"{info.TargetScore.ToString()}%";
            
            _levelPackInfoView.GalacticIcon.sprite = info.GalacticIconSprite;
            _levelPackInfoView.PassedLevels.text = levelPassText;
            _levelPackInfoView.LevelPassProgress.text = progressText;

            _levelPackBackgroundView.Background.sprite = info.GalacticBackgroundSprite;
        }

        public void UpdateProgress(int from, int to)
        {
            _scoreAnimationService.Animate(_levelPackInfoView.LevelPassProgress, from, to, SetProgress);
        }

        public void SetProgress(int progress)
        {
            _levelPackInfoView.LevelPassProgress.text = $"{progress.ToString()}%";
        }

        public void SetBackgroundSprite(Sprite backgroundSprite)
        {
            _levelPackBackgroundView.Background.sprite = backgroundSprite;
        }
    }
}