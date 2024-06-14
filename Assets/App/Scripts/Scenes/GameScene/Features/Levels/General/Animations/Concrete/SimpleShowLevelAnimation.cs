using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.GameScene.Features.Constants;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Grid;
using App.Scripts.Scenes.GameScene.Features.Levels.Loading.Loader;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.UI;

namespace App.Scripts.Scenes.GameScene.Features.Levels.General.Animations
{
    public sealed class SimpleShowLevelAnimation : IShowLevelAnimation
    {
        private readonly ILevelLoader _levelLoader;
        private readonly IGridPositionResolver _gridPositionResolver;
        private readonly Image _menuButton;
        private readonly IGridDataService _gridDataService;

        public SimpleShowLevelAnimation(
            ILevelLoader levelLoader,
            IGridPositionResolver gridPositionResolver,
            Image menuButton,
            IGridDataService gridDataService)
        {
            _levelLoader = levelLoader;
            _gridPositionResolver = gridPositionResolver;
            _menuButton = menuButton;
            _gridDataService = gridDataService;
        }
        
        public async UniTask Show()
        {
            _menuButton.raycastTarget = false;
            
            float timeOffset = GameConstants.ShowLevelDuration / _levelLoader.Entities.Count;

            List<IEntityView> sortedList = _levelLoader.Entities.OrderBy(x => x.GridPositionY).ThenBy(x => x.GridPositionX).ToList();

            int index = 0;
            foreach (IEntityView view in sortedList)
            {
                view
                    .GameObject
                    .transform
                    .DOScale(_gridDataService.CellSize, timeOffset)
                    .SetDelay(timeOffset * index - timeOffset * 0.75f)
                    .SetEase(Ease.OutBack)
                    .ToUniTask()
                    .Forget();

                index++;
            }

            await UniTask.WaitForSeconds(GameConstants.ShowLevelDuration);
            _menuButton.raycastTarget = true;
        }
    }
}