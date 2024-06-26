﻿using System.Collections.Generic;
using App.Scripts.External.Grid;
using App.Scripts.General.Infrastructure;
using App.Scripts.Scenes.GameScene.Features.Entities.AssetManagement;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Grid;

namespace App.Scripts.Scenes.GameScene.Features.Levels.General.View
{
    public interface ILevelViewUpdater : IGeneralRestartable
    {
        Grid<GridItemData> LevelGridItemData { get; }
        void SetGrid(Grid<int> grid, List<IEntityView> entityViews);
        EntityStage GetEntityStage(IEntityView entityView);
        void UpdateVisual(IEntityView entityView, int damage);
        void FastUpdateVisual(IEntityView entityView, int health);
        void SetHealth(IEntityView view, int health);
    }
}