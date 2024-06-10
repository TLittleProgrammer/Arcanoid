using App.Scripts.Scenes.GameScene.Features.Levels.General;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Grid
{
    public class GridPositionResolver : IGridPositionResolver
    {
        private readonly IGridDataService _gridDataService;

        private LevelData _levelData;
        private int2 _currentGridPosition = 0;

        public GridPositionResolver(IGridDataService gridDataService)
        {
            _gridDataService = gridDataService;
        }
        
        public async UniTask AsyncInitialize(LevelData levelData)
        {
            _gridDataService.InitializeFieldsByLevelData(levelData);

            _levelData = levelData;
            _currentGridPosition = 0;

            await UniTask.CompletedTask;
        }

        public Vector2 GetCurrentGridPosition()
        {
            Vector2 currentGridPositionInWorld = GetPositionByCoordinates(_currentGridPosition.x, _currentGridPosition.y);

            UpdateCurrentPositionIfNeed();
            
            return currentGridPositionInWorld;
        }

        public Vector2 GetPositionByCoordinates(int x, int y)
        {
            Vector2 result;

            result.x = GetXPosition(x);
            result.y = GetYPosition(y);

            return result;
        }

        public Vector2 GetCellSize()
        {
            return _gridDataService.CellSize;
        }

        public void Restart()
        {
            _currentGridPosition.x = 0;
            _currentGridPosition.y = 0;
        }

        private float GetXPosition(int x)
        {
            return _gridDataService.LeftWorldXPosition + _gridDataService.HalfCellSize.x + x * (_gridDataService.CellSize.x + _gridDataService.SpaceBetweenCells.x);
        }

        private float GetYPosition(int y)
        {
            return _gridDataService.TopPositionInWorld - y * (_gridDataService.HalfCellSize.y + _gridDataService.SpaceBetweenCells.y);
        }

        private void UpdateCurrentPositionIfNeed()
        {
            _currentGridPosition.x++;
            if (_currentGridPosition.x >= _levelData.GridSize.x)
            {
                _currentGridPosition.y++;
                _currentGridPosition.x = 0;
            }
        }
    }
}