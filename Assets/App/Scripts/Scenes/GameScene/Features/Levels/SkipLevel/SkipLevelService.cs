using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Levels.General.View;
using App.Scripts.Scenes.GameScene.Features.Levels.Loading.Loader;
using UnityEngine.UI;

namespace App.Scripts.Scenes.GameScene.Features.Levels.SkipLevel
{
    public class SkipLevelService
    {
        private readonly ILevelLoader _levelLoader;
        private readonly ILevelViewUpdater _levelViewUpdater;

        public SkipLevelService(Button button, ILevelLoader levelLoader, ILevelViewUpdater levelViewUpdater)
        {
            _levelLoader = levelLoader;
            _levelViewUpdater = levelViewUpdater;
            
            button.onClick.AddListener(RemoveAll);
        }

        private void RemoveAll()
        {
            foreach (IEntityView view in _levelLoader.Entities)
            {
                _levelViewUpdater.UpdateVisual(view, 999);
            }
        }
    }
}