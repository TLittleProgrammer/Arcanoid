using App.Scripts.Scenes.GameScene.Camera;
using App.Scripts.Scenes.GameScene.Levels;
using App.Scripts.Scenes.GameScene.ScreenInfo;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Grid
{
    public class GridPositionResolver : IGridPositionResolver
    {
        private readonly RectTransform _header;
        private readonly IScreenInfoProvider _screenInfoProvider;
        private readonly ICameraService _cameraService;

        private float _topPosition;
        private float _leftPosition;
        private float _availableSpace;
        private Vector2 _spaceBetweenCells;
        private Vector2 _cellSize;
        private LevelData _levelData;
        private int2 _curretGridPosition = 0;
        private Vector2 _worldCameraSize;

        public GridPositionResolver(RectTransform header, IScreenInfoProvider screenInfoProvider, ICameraService cameraService)
        {
            _header = header;
            _screenInfoProvider = screenInfoProvider;
            _cameraService = cameraService;
        }
        
        public async UniTask AsyncInitialize(LevelData levelData)
        {
            _worldCameraSize   = CalculateWorldCameraSize();
            _availableSpace    = CalculateAvailableSpace(levelData.HorizontalOffset, levelData.GridSize.x, levelData.OffsetBetweenCells.x);
            _spaceBetweenCells = CalculateSpaceBetweenCells(levelData.OffsetBetweenCells);
            _cellSize          = CalculateCellSize(_availableSpace, levelData.GridSize.x);
            _topPosition       = CalculateTopPosition(levelData.TopOffset);
            _leftPosition      = CalculateLeftPosition(levelData.HorizontalOffset);

            _levelData = levelData;
            _curretGridPosition = 0;

            await UniTask.CompletedTask;
        }

        public Vector2 GetCurrentGridPosition()
        {
            Vector2 result = Vector2.zero;

            result.y = _topPosition - _curretGridPosition.y * (_cellSize.y / 2f + _spaceBetweenCells.y);
            result.x = _leftPosition + _cellSize.x / 2f + _curretGridPosition.x * (_cellSize.x + _spaceBetweenCells.x);

            _curretGridPosition.x++;
            if (_curretGridPosition.x >= _levelData.GridSize.x)
            {
                _curretGridPosition.y++;
                _curretGridPosition.x = 0;
            }
            
            return result;
        }

        private Vector2 CalculateWorldCameraSize()
        {
            Vector2 topRightPoint = _cameraService.ScreenToWorldPoint(new Vector2(_screenInfoProvider.WidthInPixels, _screenInfoProvider.HeightInPixels));
            topRightPoint *= 2;

            return topRightPoint;
        }

        private float CalculateLeftPosition(int horizontalOffset)
        {
            float leftSide = -_worldCameraSize.x / 2f;

            leftSide += _worldCameraSize.x * (horizontalOffset / 100f / 2f);
            
            return leftSide;
        }

        public Vector2 GetCellSize()
        {
            return _cellSize;
        }

        private Vector2 CalculateCellSize(float availableSpace, int gridSizeX)
        {
            Vector2 size = Vector2.one;

            size.x = availableSpace / gridSizeX;
            size.y = size.x;
                
            return size;
        }

        private Vector2 CalculateSpaceBetweenCells(int2 offsetBetweenCells)
        {
            return new Vector2(
                _worldCameraSize.x * (offsetBetweenCells.x / 100f),
                _worldCameraSize.y * (offsetBetweenCells.y / 100f)
            );
        }

        private float CalculateAvailableSpace(int levelDataHorizontalOffset, int gridSize, int offset)
        {
            return _worldCameraSize.x - _worldCameraSize.x * (levelDataHorizontalOffset / 100f) - _worldCameraSize.x * offset / 100f * (gridSize - 1);
        }

        private float CalculateTopPosition(float topOffset)
        {
            Vector2 bottomLeftCorner = new Vector2(_header.rect.xMin, _header.rect.yMin);
            Vector3 worldBottomLeftCorner = _header.TransformPoint(bottomLeftCorner);
            float positionInPixels = _screenInfoProvider.HeightInPixels - _screenInfoProvider.HeightInPixels * (topOffset / 100f);
            
            return worldBottomLeftCorner.y - (_worldCameraSize.y / 2f - _cameraService.ScreenToWorldPoint(new(0f, positionInPixels)).y) - _cellSize.y / 2f;
        }

        public void Restart()
        {
            _curretGridPosition.x = 0;
            _curretGridPosition.y = 0;
        }
    }
}