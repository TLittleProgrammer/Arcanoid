using System;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Levels.General.View;
using App.Scripts.Scenes.GameScene.Features.Levels.Loading.Loader;

namespace App.Scripts.Scenes.GameScene.Features.Levels.SkipLevel
{
    public class SkipLevelService : ISkipLevelService
    {
        private readonly ILevelLoader _levelLoader;
        private readonly ILevelViewUpdater _levelViewUpdater;

        public SkipLevelService(ILevelLoader levelLoader, ILevelViewUpdater levelViewUpdater)
        {
            _levelLoader = levelLoader;
            _levelViewUpdater = levelViewUpdater;
        }

        public void Skip()
        {
            RemoveAll();
        }

        private void RemoveAll()
        {
            foreach (IEntityView view in _levelLoader.Entities)
            {
                _levelViewUpdater.UpdateVisual(view, Int32.MaxValue);
            }
        }
    }
}