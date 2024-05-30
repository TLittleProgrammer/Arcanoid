using App.Scripts.External.Initialization;
using App.Scripts.General.Infrastructure;
using App.Scripts.Scenes.GameScene.Features.Levels.General;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Grid
{
    public interface IGridPositionResolver : IAsyncInitializable<LevelData>, IGeneralRestartable
    {
        Vector2 GetCurrentGridPosition();
        Vector2 GetCellSize();
        Vector2 GetPositionByCoordinates(int x, int y);
    }
}