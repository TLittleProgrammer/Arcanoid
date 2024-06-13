using System.Collections;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.GameScene.Features.Constants;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Grid;
using App.Scripts.Scenes.GameScene.Features.Levels.General.View;
using App.Scripts.Scenes.GameScene.Features.Levels.Loading.Loader;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.UI;

namespace App.Scripts.Scenes.GameScene.Features.Levels.General.Animations
{
    public class AnimationInDiagonals : IShowLevelAnimation
    {
        private readonly ILevelLoader _levelLoader;
        private readonly IGridPositionResolver _gridPositionResolver;
        private readonly ILevelViewUpdater _levelViewUpdater;
        private readonly Image _menuButton;

        public AnimationInDiagonals(
            ILevelLoader levelLoader,
            IGridPositionResolver gridPositionResolver,
            ILevelViewUpdater levelViewUpdater,
            Image menuButton)
        {
            _levelLoader = levelLoader;
            _gridPositionResolver = gridPositionResolver;
            _levelViewUpdater = levelViewUpdater;
            _menuButton = menuButton;
        }
        
        public UniTask Show()
        {
            _menuButton.raycastTarget = false;
            
            float timeOffset = GameConstants.ShowLevelDuration / _levelLoader.Entities.Count;
            
            List<IEntityView> sortedList = TraverseDiagonals();

            int index = 0;
            foreach (IEntityView view in sortedList)
            {
                view
                    .GameObject
                    .transform
                    .DOScale(_gridPositionResolver.GetCellSize(), timeOffset)
                    .SetDelay(timeOffset * index - timeOffset * 0.65f)
                    .SetEase(Ease.OutBack)
                    .ToUniTask()
                    .Forget();

                index++;
            }

            _menuButton.raycastTarget = true;
            return UniTask.CompletedTask;
        }
        
        private List<IEntityView> TraverseDiagonals()
        {
            int rows = _levelViewUpdater.LevelGridItemData.Height;
            int columns = _levelViewUpdater.LevelGridItemData.Width;

            List<IEntityView> views = new();
            
            for (int row = rows - 1; row >= 0; row--)
            {
                ThroughDiagonal(row, 0, rows, columns, views);
            }
            
            for (int col = 1; col < columns; col++)
            {
                ThroughDiagonal(0, col, rows, columns, views);
            }

            return views;
        }

        private void ThroughDiagonal(int startRow, int startCol, int allRows, int allColumns, List<IEntityView> views)
        {
            int currentRow = startRow;
            int currentColumn = startCol;

            while (currentRow < allRows && currentColumn < allColumns)
            {
                IEntityView view = _levelLoader.Entities.FirstOrDefault(x => x.GridPositionX == currentColumn && x.GridPositionY == currentRow);

                if (view is not null)
                {
                    views.Add(view);
                }

                currentRow++;
                currentColumn++;
            }
        }
    }
}