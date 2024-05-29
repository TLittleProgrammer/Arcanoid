using System;
using System.Threading.Tasks;
using App.Scripts.Scenes.GameScene.Features.Grid;
using App.Scripts.Scenes.GameScene.Features.Levels.General;
using App.Scripts.Scenes.GameScene.Features.Levels.General.Load;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.Features.Levels.Loading
{
    public sealed class LevelLoadService : ILevelLoadService
    {
        private readonly ILevelDataChooser _levelDataChooser;
        private readonly IGridPositionResolver _gridPositionResolver;
        private readonly IMechanicsByLevelActivator _mechanicsByLevelActivator;
        private readonly ILevelLoader _levelLoader;

        public LevelLoadService(
            ILevelDataChooser levelDataChooser,
            IGridPositionResolver gridPositionResolver,
            IMechanicsByLevelActivator mechanicsByLevelActivator,
            ILevelLoader levelLoader)
        {
            _levelDataChooser = levelDataChooser;
            _gridPositionResolver = gridPositionResolver;
            _mechanicsByLevelActivator = mechanicsByLevelActivator;
            _levelLoader = levelLoader;
        }

        public event Action<LevelData> LevelLoaded;

        public async UniTask<LevelData> LoadLevel()
        {
            LevelData levelData = _levelDataChooser.GetLevelData();
            
            return await LoadChoosedLevel(levelData);
        }
        
        public async UniTask<LevelData> LoadLevelNextLevel()
        {
            LevelData levelData = _levelDataChooser.GetNextLevelData();

            return await LoadChoosedLevel(levelData);
        }

        private async UniTask<LevelData> LoadChoosedLevel(LevelData levelData)
        {
            await _gridPositionResolver.AsyncInitialize(levelData);
            _mechanicsByLevelActivator.ActivateByLevelData(levelData);

            _levelLoader.LoadLevel(levelData);

            LevelLoaded?.Invoke(levelData);
            return levelData;
        }
    }
}