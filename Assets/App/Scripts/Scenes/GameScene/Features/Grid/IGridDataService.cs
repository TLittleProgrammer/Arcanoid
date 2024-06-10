using App.Scripts.Scenes.GameScene.Features.Levels.General;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Grid
{
    public interface IGridDataService
    {
        float LeftWorldXPosition { get; }
        Vector2 CellSize { get; }
        Vector2 HalfCellSize { get; }
        Vector2 SpaceBetweenCells { get; }
        float TopPositionInWorld { get; }
        void InitializeFieldsByLevelData(LevelData levelData);
    }
}