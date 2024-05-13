using App.Scripts.External.Initialization;
using App.Scripts.Scenes.GameScene.Infrastructure;
using App.Scripts.Scenes.GameScene.Levels;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Grid
{
    public interface IGridPositionResolver : IAsyncInitializable<LevelData>, IRestartable
    {
        Vector2 GetCurrentGridPosition();
        Vector2 GetCellSize();
    }
}