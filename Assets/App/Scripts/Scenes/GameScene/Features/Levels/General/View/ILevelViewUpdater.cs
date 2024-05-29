using System.Collections.Generic;
using App.Scripts.External.Grid;
using App.Scripts.Scenes.GameScene.Features.Entities.AssetManagement;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Grid;

namespace App.Scripts.Scenes.GameScene.Features.Levels.General.View
{
    public interface ILevelViewUpdater
    {
        Grid<GridItemData> LevelGridItemData { get; }
        void SetGrid(Grid<int> grid, List<IEntityView> entityViews);
        EntityStage GetEntityStage(IEntityView entityView);
        void UpdateVisual(IEntityView entityView, int damage);
    }
}