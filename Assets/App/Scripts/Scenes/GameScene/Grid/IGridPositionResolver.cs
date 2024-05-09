using App.Scripts.External.Initialization;
using App.Scripts.Scenes.GameScene.Levels;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Grid
{
    public interface IGridPositionResolver : IAsyncInitializable<LevelData>
    {
        Vector2 GetCurrentGridPosition();
        Vector2 GetCellSize();
    }
}