using App.Scripts.External.Initialization;
using App.Scripts.General.Game;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress
{
    public class LevelProgressSaveHandler : IInitializable, IDisposable
    {
        private readonly IGameStatements _gameStatements;
        private readonly ILevelProgressSaveService _levelProgressSaveService;

        public LevelProgressSaveHandler(IGameStatements gameStatements, ILevelProgressSaveService levelProgressSaveService)
        {
            _gameStatements = gameStatements;
            _levelProgressSaveService = levelProgressSaveService;
        }

        public void Initialize()
        {
            _gameStatements.ApplicationQuit += OnApplicationQuit;
        }

        private void OnApplicationQuit()
        {
            _levelProgressSaveService.SaveProgress();
        }

        public void Dispose()
        {
            _gameStatements.ApplicationQuit -= OnApplicationQuit;
        }
    }
}