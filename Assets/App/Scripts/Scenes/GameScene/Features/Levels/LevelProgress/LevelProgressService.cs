﻿using System;
using App.Scripts.General.LevelPackInfoService;
using App.Scripts.General.Providers;
using App.Scripts.Scenes.GameScene.Features.Entities.AssetManagement;
using App.Scripts.Scenes.GameScene.Features.Levels.General;
using App.Scripts.Scenes.GameScene.Features.Levels.LevelView;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress;
using App.Scripts.Scenes.GameScene.Features.ScoreAnimation;
using TMPro;
using UnityEngine.Playables;

namespace App.Scripts.Scenes.GameScene.Features.Levels.LevelProgress
{
    public class LevelProgressService : ILevelProgressService, ILevelProgressSavable, IInitializeByLevelProgress
    {
        private readonly ILevelPackInfoService _levelPackInfoService;
        private readonly ILevelPackInfoView _levelPackInfoView;
        private readonly ILevelPackBackgroundView _levelPackBackgroundView;
        private readonly EntityProvider _entitesProvider;
        private readonly IScoreAnimationService _scoreAnimationService;
        private readonly SpriteProvider _spriteProvider;

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
            IScoreAnimationService scoreAnimationService,
            SpriteProvider spriteProvider)
        {
            _levelPackInfoService = levelPackInfoService;
            _levelPackInfoView = levelPackInfoView;
            _levelPackBackgroundView = levelPackBackgroundView;
            _entitesProvider = entitesProvider;
            _scoreAnimationService = scoreAnimationService;
            _spriteProvider = spriteProvider;
        }

        public void Initialize()
        {
            var data = _levelPackInfoService.GetData();
            if (data is not null && data.NeedLoadLevel)
            {
                _levelPackBackgroundView.Background.sprite = _spriteProvider.Sprites[data.LevelPack.GalacticBackgroundKey];

                _targetScore = 0;
                _levelPackInfoView.Initialize(new()
                {
                    CurrentLevelIndex = data.LevelIndex,
                    AllLevelsCountFromPack = data.LevelPack.Levels.Count,
                    Sprite = _spriteProvider.Sprites[data.LevelPack.GalacticIconKey],
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

        public void SaveProgress(LevelDataProgress levelDataProgress)
        {
            ProgressedLevelData progressedLevelData = new();

            progressedLevelData.DestroyedBlocks = _destroyedBlockCounter;
            progressedLevelData.Progress = _progress;
            progressedLevelData.Step = _step;
            progressedLevelData.AllBlocksCounter = _allBlockCounter;

            levelDataProgress.ProgressedLevelData = progressedLevelData;
        }

        public void LoadProgress(LevelDataProgress levelDataProgress)
        {
            ProgressedLevelData progressedLevelData = levelDataProgress.ProgressedLevelData;

            _destroyedBlockCounter = progressedLevelData.DestroyedBlocks;
            _progress = progressedLevelData.Progress;
            _step = progressedLevelData.Step;
            _allBlockCounter = progressedLevelData.AllBlocksCounter;
            
            _targetScore = (int)Math.Round(_progress * 100f);
            
            _levelPackInfoView.UpdateProgressText(_targetScore);
        }
    }
}