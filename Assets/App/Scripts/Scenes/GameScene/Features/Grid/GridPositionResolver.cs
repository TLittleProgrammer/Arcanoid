using App.Scripts.Scenes.GameScene.Features.Levels.General;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Grid
{
    public class GridPositionResolver : IGridPositionResolver
    {
        private readonly IGridDataService _gridDataService;
        public GridPositionResolver(IGridDataService gridDataService)
        {
            _gridDataService = gridDataService;
        }
        
        public async UniTask AsyncInitialize(LevelData levelData)
        {
            _gridDataService.InitializeFieldsByLevelData(levelData);

            await UniTask.CompletedTask;
        }

        public Vector2 GetPositionByCoordinates(int x, int y)
        {
            Vector2 result;

            result.x = GetXPosition(x);
            result.y = GetYPosition(y);

            return result;
        }

        private float GetXPosition(int x)
        {
            return _gridDataService.LeftWorldXPosition + _gridDataService.HalfCellSize.x + x * (_gridDataService.CellSize.x + _gridDataService.SpaceBetweenCells.x);
        }

        private float GetYPosition(int y)
        {
            return _gridDataService.TopPositionInWorld - y * (_gridDataService.HalfCellSize.y + _gridDataService.SpaceBetweenCells.y);
        }
    }
}