using System;
using App.Scripts.General.Levels;
using App.Scripts.Scenes.GameScene.Levels;
using App.Scripts.Scenes.GameScene.Levels.AssetManagement;
using App.Scripts.Scenes.GameScene.LevelView;
using App.Scripts.Scenes.GameScene.ScoreAnimation;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.LevelProgress
{
    public class LevelProgressService : ILevelProgressService
    {
        private readonly ILevelPackTransferData _levelPackTransferData;
        private readonly ILevelPackInfoView _levelPackInfoView;
        private readonly ILevelPackBackgroundView _levelPackBackgroundView;
        private readonly EntityProvider _entitesProvider;
        private readonly IScoreAnimationService _scoreAnimationService;

        private float _step;
        private float _progress;
        private int _allBlockCounter;
        private int _destroyedBlockCounter;
        private int _targetScore;

        public event Action LevelPassed;
        
        public LevelProgressService(
            ILevelPackTransferData levelPackTransferData,
            ILevelPackInfoView levelPackInfoView,
            ILevelPackBackgroundView levelPackBackgroundView,
            EntityProvider entitesProvider,
            IScoreAnimationService scoreAnimationService)
        {
            _levelPackTransferData = levelPackTransferData;
            _levelPackInfoView = levelPackInfoView;
            _levelPackBackgroundView = levelPackBackgroundView;
            _entitesProvider = entitesProvider;
            _scoreAnimationService = scoreAnimationService;
        }

        public void Initialize()
        {
            if (_levelPackTransferData.NeedLoadLevel)
            {
                _levelPackBackgroundView.Background.sprite = _levelPackTransferData.LevelPack.GalacticBackground;
                _levelPackInfoView.Image.sprite = _levelPackTransferData.LevelPack.GalacticIcon;
                _levelPackInfoView.PassedLevels.text = $"{_levelPackTransferData.LevelIndex}/{_levelPackTransferData.LevelPack.Levels.Count}";

                _targetScore = 0;
                UpdateProgressText(_targetScore);
            }
        }

        public void TakeOneStep()
        {
            _destroyedBlockCounter++;

            if (_destroyedBlockCounter == _allBlockCounter)
            {
                _progress = 1f;
                _scoreAnimationService.Animate(_levelPackInfoView.LevelPassProgress, _targetScore, 100, UpdateProgressText);

                LevelPassed?.Invoke();
                
                return;
            }
            
            _progress += _step;
            _targetScore = (int)Math.Round(_progress * 100f);
            
            _scoreAnimationService.Animate(_levelPackInfoView.LevelPassProgress, (int)((_progress - _step) * 100), _targetScore, UpdateProgressText);
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
            
            UpdateProgressText(_targetScore);
        }

        private void UpdateProgressText(int value)
        {
            _levelPackInfoView.LevelPassProgress.text = $"{value}%";
        }
    }
}