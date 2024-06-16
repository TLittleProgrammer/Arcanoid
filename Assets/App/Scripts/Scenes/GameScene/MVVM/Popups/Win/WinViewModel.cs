using App.Scripts.General.Levels;
using App.Scripts.General.Levels.LevelPackInfoService;
using App.Scripts.General.Providers;
using App.Scripts.Scenes.GameScene.Features.Popups;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.MVVM.Popups.Win
{
    public class WinViewModel
    {
        private readonly ILevelPackInfoService _levelPackInfoService;
        private readonly SpriteProvider _spriteProvider;
        
        public WinViewModel(ILevelPackInfoService levelPackInfoService, SpriteProvider spriteProvider)
        {
            _levelPackInfoService = levelPackInfoService;
            _spriteProvider = spriteProvider;
        }

        public WinViewRecord GetViewRecord()
        {
            LevelPack currentPack = _levelPackInfoService.LevelPackTransferData.LevelPack;
            LevelPack nextPack = _levelPackInfoService.GetDataForNextPack();

            Sprite nextPackSprite = nextPack is null ? null : _spriteProvider.Sprites[nextPack.GalacticIconKey];
            
            return new(
                _spriteProvider.Sprites[currentPack.GalacticIconKey],
                nextPackSprite,
                currentPack.LocaleKey,
                _levelPackInfoService.LevelPackTransferData.LevelIndex + 1,
                currentPack.Levels.Count
                );
        }

        public bool NeedLoadNextPack()
        {
            return _levelPackInfoService.NeedLoadNextPack();
        }

        public LevelPack GetNextLevelPack()
        {
            return _levelPackInfoService.GetDataForNextPack();
        }
        
        public LevelPack GetCurrentLevelPack()
        {
            return _levelPackInfoService.GetDataForCurrentPack();
        }
    }
}