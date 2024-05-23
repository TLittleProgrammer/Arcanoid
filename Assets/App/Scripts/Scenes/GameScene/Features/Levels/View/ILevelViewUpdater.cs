using App.Scripts.External.Grid;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Levels.AssetManagement;
using App.Scripts.Scenes.GameScene.Features.Levels.Data;

namespace App.Scripts.Scenes.GameScene.Features.Levels.View
{
    public interface ILevelViewUpdater
    {
        Grid<GridItemData> LevelGridItemData { get; }
        void SetGrid(Grid<int> grid);
        EntityStage GetEntityStage(IEntityView entityView);
        void UpdateVisual(IEntityView entityView);
    }
}