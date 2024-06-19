using App.Scripts.General.Levels.LevelPackInfoService;
using App.Scripts.General.Providers;
using App.Scripts.Scenes.GameScene.Features.Entities.Bird.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Levels.General;
using App.Scripts.Scenes.GameScene.Features.Levels.LevelProgress;
using App.Scripts.Scenes.GameScene.Features.Levels.Loading;
using App.Scripts.Scenes.GameScene.MVVM.Header;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.Features.Restart
{
    public class UpdateServiceForNewLevel : IUpdateServiceForNewLevel
    {
        private readonly ILevelLoadService _levelLoadService;
        
        private readonly SpriteProvider _spriteProvider;
        private readonly LevelPackInfoViewModel _levelPackInfoViewModel;
        private readonly ILevelPackInfoService _levelPackInfoService;
        private readonly ILevelProgressService _levelProgressService;
        private readonly IBirdsService _birdsService;

        public UpdateServiceForNewLevel(
            ILevelLoadService levelLoadService,
            SpriteProvider spriteProvider,
            LevelPackInfoViewModel levelPackInfoViewModel,
            ILevelPackInfoService levelPackInfoService,
            ILevelProgressService levelProgressService,
            IBirdsService birdsService)
        {
            _levelLoadService = levelLoadService;
            _spriteProvider = spriteProvider;
            _levelPackInfoViewModel = levelPackInfoViewModel;
            _levelPackInfoService = levelPackInfoService;
            _levelProgressService = levelProgressService;
            _birdsService = birdsService;
        }
        
        public async UniTask Update()
        {
            LevelData level = await _levelLoadService.LoadLevelNextLevel();

            var data = _levelPackInfoService.LevelPackTransferData;
            
            _levelProgressService.CalculateStepByLevelData(level);
            
            _levelPackInfoViewModel.UpdateView(new LevelPackInfoRecord
            {
                AllLevelsCountFromPack = data.LevelPack.Levels.Count,
                CurrentLevelIndex = data.LevelIndex,
                GalacticIconSprite = _spriteProvider.Sprites[data.LevelPack.GalacticIconKey],
                GalacticBackgroundSprite = _spriteProvider.Sprites[data.LevelPack.GalacticBackgroundKey],
                TargetScore = 0
            });
        }
    }
}