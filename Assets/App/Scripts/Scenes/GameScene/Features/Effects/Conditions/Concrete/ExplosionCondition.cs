using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Grid;
using App.Scripts.Scenes.GameScene.Features.Levels.General.View;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Effects.Conditions
{
    public sealed class ExplosionCondition : ICondition
    {
        private readonly ILevelViewUpdater _levelViewUpdater;

        public ExplosionCondition(ILevelViewUpdater levelViewUpdater)
        {
            _levelViewUpdater = levelViewUpdater;
        }
        
        public bool Execute(IEntityView entityView, Collision2D collision)
        {
            GridItemData gridItemData = _levelViewUpdater.LevelGridItemData[entityView.GridPositionX, entityView.GridPositionY];
            
            return gridItemData.CurrentHealth < 0;
        }
    }
}