using System;
using App.Scripts.General.Levels;
using App.Scripts.General.Levels.LevelPackInfoService;
using App.Scripts.General.Providers;
using App.Scripts.Scenes.GameScene.Features.Entities.AssetManagement;
using App.Scripts.Scenes.GameScene.Features.Levels.General;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress.Data;
using App.Scripts.Scenes.GameScene.MVVM.Header;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Levels.LevelProgress
{
    public class LevelPackInfoModel : ILevelProgressService, ILevelProgressSavable, IInitializeByLevelProgress
    {
        private readonly ILevelPackInfoService _levelPackInfoService;
        private readonly EntityProvider _entitesProvider;
        private readonly SpriteProvider _spriteProvider;
        private readonly LevelPackInfoViewModel _viewModel;

        private float _step;
        private float _progress;
        private int _allBlockCounter;
        private int _destroyedBlockCounter;
        private int _progressInPercents;
        private const int AbsenceEntityIndex = 0; 

        public event Action<float> ProgressChanged;
        public event Action LevelPassed;
        
        public LevelPackInfoModel(
            ILevelPackInfoService levelPackInfoService,
            EntityProvider entitesProvider,
            SpriteProvider spriteProvider,
            LevelPackInfoViewModel viewModel
            )
        {
            _levelPackInfoService = levelPackInfoService;
            _entitesProvider = entitesProvider;
            _spriteProvider = spriteProvider;
            _viewModel = viewModel;
        }

        public void Initialize()
        {
            var levelTransferData = _levelPackInfoService.LevelPackTransferData;
            
            if (levelTransferData is not null && levelTransferData.NeedLoadLevel)
            {
                _progressInPercents = 0;
                UpdateVisual(levelTransferData);
            }
        }

        private void UpdateVisual(ILevelPackTransferData levelTransferData)
        {
            _viewModel.UpdateView(new()
            {
                CurrentLevelIndex = levelTransferData.LevelIndex,
                AllLevelsCountFromPack = levelTransferData.LevelPack.Levels.Count,
                GalacticIconSprite = GetSpriteByKey(levelTransferData.LevelPack.GalacticIconKey),
                TargetScore = _progressInPercents
            });
            
            Sprite backgroundSprite = GetSpriteByKey(levelTransferData.LevelPack.GalacticBackgroundKey);
            _viewModel.SetBackgroundSprite(backgroundSprite);
        }

        public void TakeOneStep()
        {
            _destroyedBlockCounter++;

            if (IsDestroyedAll())
            {
                return;
            }
            
            UpdateDataAndViewByStep();
        }

        private bool IsDestroyedAll()
        {
            if (_destroyedBlockCounter == _allBlockCounter)
            {
                _progress = 1f;
                _viewModel.UpdateProgress(_progressInPercents, 100);
                LevelPassed?.Invoke();

                return true;
            }

            return false;
        }

        private void UpdateDataAndViewByStep()
        {
            _progress += _step;

            _progressInPercents = CalculateProgressInPercents(_progress);
            _viewModel.UpdateProgress(CalculateProgressInPercents(_progress - _step), _progressInPercents);

            ProgressChanged?.Invoke(_progress);
        }

        public void CalculateStepByLevelData(LevelData levelData)
        {
            int damagableCounter = 0;
            
            foreach (int index in levelData.Grid)
            {
                if (index != AbsenceEntityIndex && _entitesProvider.EntityStages[index.ToString()].ICanGetDamage)
                {
                    damagableCounter++;
                }
            }

            _allBlockCounter = damagableCounter;
            _step = 1f / damagableCounter;
        }

        public void Restart()
        {
            _progress = 0f;
            _destroyedBlockCounter = 0;
            _progressInPercents = 0;
            
            _viewModel.SetProgress(0);
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
            
            _progressInPercents = CalculateProgressInPercents(_progress);
            _viewModel.SetProgress(_progressInPercents);
        }

        private int CalculateProgressInPercents(float progress)
        {
            return (int)Math.Round(progress * 100f);
        }

        private Sprite GetSpriteByKey(string key)
        {
            return _spriteProvider.Sprites[key];
        }
    }
}