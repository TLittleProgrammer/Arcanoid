using System;
using App.Scripts.General.LevelPackInfoService;
using App.Scripts.Scenes.GameScene.Features.Levels;
using App.Scripts.Scenes.GameScene.Features.Levels.AssetManagement;
using App.Scripts.Scenes.GameScene.Features.LevelView;
using App.Scripts.Scenes.GameScene.Features.ScoreAnimation;
using TMPro;

namespace App.Scripts.Scenes.GameScene.Features.LevelProgress
{
    public class LevelProgressService : ILevelProgressService
    {
        private readonly ILevelPackInfoService _levelPackInfoService;
        private readonly ILevelPackInfoView _levelPackInfoView;
        private readonly ILevelPackBackgroundView _levelPackBackgroundView;
        private readonly EntityProvider _entitesProvider;
        private readonly IScoreAnimationService _scoreAnimationService;

        private float _step;
        private float _progress;
        private int _allBlockCounter;
        private int _destroyedBlockCounter;
        private int _targetScore;

        public event Action<float> ProgressChanged;
        public event Action LevelPassed;
        
        public LevelProgressService(
            ILevelPackInfoService levelPackInfoService,
            ILevelPackInfoView levelPackInfoView,
            ILevelPackBackgroundView levelPackBackgroundView,
            EntityProvider entitesProvider,
            IScoreAnimationService scoreAnimationService)
        {
            _levelPackInfoService = levelPackInfoService;
            _levelPackInfoView = levelPackInfoView;
            _levelPackBackgroundView = levelPackBackgroundView;
            _entitesProvider = entitesProvider;
            _scoreAnimationService = scoreAnimationService;
        }

        public void Initialize()
        {
            var data = _levelPackInfoService.GetData();
            if (data is not null && data.NeedLoadLevel)
            {
                _levelPackBackgroundView.Background.sprite = data.LevelPack.GalacticBackground;

                _targetScore = 0;
                _levelPackInfoView.Initialize(new()
                {
                    CurrentLevelIndex = data.LevelIndex,
                    AllLevelsCountFromPack = data.LevelPack.Levels.Count,
                    Sprite = data.LevelPack.GalacticIcon,
                    TargetScore = _targetScore
                });
            }
        }

        public void TakeOneStep()
        {
            _destroyedBlockCounter++;

            if (_destroyedBlockCounter == _allBlockCounter)
            {
                _progress = 1f;
                AnimateScore(_levelPackInfoView.LevelPassProgress, _targetScore, 100, _levelPackInfoView.UpdateProgressText);
                LevelPassed?.Invoke();
                
                return;
            }
            
            _progress += _step;
            _targetScore = (int)Math.Round(_progress * 100f);
            
            ProgressChanged?.Invoke(_progress);
            
            AnimateScore(_levelPackInfoView.LevelPassProgress, (int)((_progress - _step) * 100), _targetScore, _levelPackInfoView.UpdateProgressText);
        }

        public void CalculateStepByLevelData(LevelData levelData)
        {
            int damagableCounter = 0;
            
            foreach (int index in levelData.Grid)
            {
                if (index != 0)
                {
                    if (_entitesProvider.EntityStages[index.ToString()].ICanGetDamage)
                    {
                        damagableCounter++;
                    }
                }
            }

            _allBlockCounter = damagableCounter;
            _step = 1f / damagableCounter;
        }

        public void Restart()
        {
            _progress = 0f;
            _destroyedBlockCounter = 0;
            _targetScore = 0;
            
            _levelPackInfoView.UpdateProgressText(_targetScore);
        }

        private void AnimateScore(TMP_Text text, int from, int to, Action<int> ticked)
        {
            _scoreAnimationService.Animate(text, from, to, ticked);
        }
    }
}