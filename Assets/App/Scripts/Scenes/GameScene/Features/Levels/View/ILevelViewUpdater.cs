using App.Scripts.External.Grid;
using App.Scripts.Scenes.GameScene.Entities;
using App.Scripts.Scenes.GameScene.Levels.AssetManagement;
using App.Scripts.Scenes.GameScene.Levels.Data;

namespace App.Scripts.Scenes.GameScene.Levels.View
{
    public interface ILevelViewUpdater
    {
        Grid<GridItemData> LevelGridItemData { get; }
        void SetGrid(Grid<int> grid);
        EntityStage GetEntityStage(IEntityView entityView);
        void UpdateVisual(IEntityView entityView);
    }
}