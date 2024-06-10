using App.Scripts.Scenes.GameScene.Features.Camera;
using App.Scripts.Scenes.GameScene.Features.Levels.General;
using App.Scripts.Scenes.GameScene.Features.ScreenInfo;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Grid
{
    public sealed class GridDataService : IGridDataService, IInitializable
    {
        private readonly IScreenInfoProvider _screenInfoProvider;
        private readonly RectTransform _header;
        private readonly ICameraService _cameraService;

        private float _leftCameraSidePositionX;
        private Vector3 _bottomLeftCornerHeaderInWorld;
        
        private const float BlockPixelsPerUnit = 100f;
        
        public GridDataService(
            RectTransform header,
            IScreenInfoProvider screenInfoProvider,
            ICameraService cameraService)
        {
            _screenInfoProvider = screenInfoProvider;
            _header = header;
            _cameraService = cameraService;
        }

        public float LeftWorldXPosition { get; private set; }
        public Vector2 CellSize { get; private set; }
        public Vector2 HalfCellSize { get; private set; }
        public Vector2 SpaceBetweenCells { get; private set; }

        public float TopPositionInWorld { get; private set; }

        public void Initialize()
        {
            _leftCameraSidePositionX = -_screenInfoProvider.ScreenWorldSize.x / 2f;
            _bottomLeftCornerHeaderInWorld = CalculateBottomLeftCornerHeaderPositionInWorld();
        }

        public void InitializeFieldsByLevelData(LevelData levelData)
        {
            LeftWorldXPosition = CalculateLeftWorldXPosition(levelData.HorizontalOffset);
            CellSize           = CalculateCellSize(levelData);
            SpaceBetweenCells  = CalculateSpaceBetweenCells(levelData.OffsetBetweenCells);
            HalfCellSize       = CellSize / 2f;
            TopPositionInWorld = CalculateTopPositionInWorld(levelData.TopOffset);
        }

        private Vector3 CalculateBottomLeftCornerHeaderPositionInWorld()
        {
            Vector2 bottomLeftCorner = new Vector2(_header.rect.xMin, _header.rect.yMin);
            
            return _header.TransformPoint(bottomLeftCorner);
        }

        private float CalculateLeftWorldXPosition(int horizontalOffset)
        {
            return _leftCameraSidePositionX + _screenInfoProvider.ScreenWorldSize.x * (horizontalOffset / BlockPixelsPerUnit / 2f);
        }

        private Vector2 CalculateCellSize(LevelData levelData)
        {
            float availableXSpace = CalculateAvailableSpace(levelData.HorizontalOffset, levelData.GridSize.x, levelData.OffsetBetweenCells.x);

            return CalculateCellSize(availableXSpace, levelData.GridSize.x);
        }

        private float CalculateAvailableSpace(int levelDataHorizontalOffset, int gridSizeX, int offsetBetweenXCells)
        {
            return _screenInfoProvider.ScreenWorldSize.x * (1 - levelDataHorizontalOffset / BlockPixelsPerUnit - offsetBetweenXCells / BlockPixelsPerUnit * (gridSizeX - 1));
        }

        private Vector2 CalculateCellSize(float availableSpace, int gridSizeX)
        {
            Vector2 cellSize;

            cellSize.x = availableSpace / gridSizeX;
            cellSize.y = cellSize.x;

            return cellSize;
        }

        private Vector2 CalculateSpaceBetweenCells(int2 offsetBetweenCells)
        {
            return new Vector2(
                _screenInfoProvider.ScreenWorldSize.x * (offsetBetweenCells.x / BlockPixelsPerUnit),
                _screenInfoProvider.ScreenWorldSize.y * (offsetBetweenCells.y / BlockPixelsPerUnit)
            );
        }

        private float CalculateTopPositionInWorld(float topOffset)
        {
            float positionInPixels = _screenInfoProvider.HeightInPixels - _screenInfoProvider.HeightInPixels * (topOffset / BlockPixelsPerUnit);
            
            return _bottomLeftCornerHeaderInWorld.y - (_screenInfoProvider.ScreenWorldSize.y / 2f - _cameraService.ScreenToWorldPoint(new(0f, positionInPixels)).y) - HalfCellSize.y;
        }
    }
}