using System.Linq;
using App.Scripts.External.Extensions.ListExtensions;
using App.Scripts.External.Grid;
using App.Scripts.Scenes.GameScene.Components;
using App.Scripts.Scenes.GameScene.Containers;
using App.Scripts.Scenes.GameScene.Entities;
using App.Scripts.Scenes.GameScene.Levels.AssetManagement;
using App.Scripts.Scenes.GameScene.Pools;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Levels.View
{
    public class LevelViewUpdater : ILevelViewUpdater
    {
        private readonly EntityProvider _entityProvider;
        private readonly IContainer<IBoxColliderable2D> _boxColliderContainer;
        private readonly IPoolContainer _poolContainer;

        private Grid<int> _levelGrid;

        public LevelViewUpdater(
            EntityProvider entityProvider,
            IContainer<IBoxColliderable2D> boxColliderContainer,
            IPoolContainer poolContainer)
        {
            _entityProvider = entityProvider;
            _boxColliderContainer = boxColliderContainer;
            _poolContainer = poolContainer;
        } 

        public void SetGrid(Grid<int> grid)
        {
            _levelGrid = grid;
        }

        public void UpdateVisual(IEntityView entityView)
        {
            string index = GetIndexByEntityView(entityView);
            
            EntityStage entityStage = _entityProvider.EntityStages[index];

            if (entityStage.ICanGetDamage)
            {
                if (entityStage.HealthCounter - 1 <= 0)
                {
                    _boxColliderContainer.RemoveItem(entityView);
                    _poolContainer.RemoveItem(PoolTypeId.EntityView, entityView as EntityView);
                }
                else
                {
                    EntityStage nextStage = entityStage.NextStage;

                    entityView.MainSprite  = nextStage.Sprite;
                    entityView.OnTopSprite = nextStage.AvailableSpritesOnMainSprite.GetRandomValue();

                    Debug.Log($"GridPosition1: {entityView.GridPositionX}; {entityView.GridPositionY}. Value = {_levelGrid[entityView.GridPositionX, entityView.GridPositionY]}");
                    _levelGrid[entityView.GridPositionX, entityView.GridPositionY] = GetIndexByStage(nextStage);
                    Debug.Log($"GridPosition2: {entityView.GridPositionX}; {entityView.GridPositionY}, Value = {_levelGrid[entityView.GridPositionX, entityView.GridPositionY]}");
                }
            }
        }

        private int GetIndexByStage(EntityStage entityStage)
        {
            return int.Parse(_entityProvider.EntityStages.First(x => x.Value.Equals(entityStage)).Key);
        }

        private string GetIndexByEntityView(IEntityView entityView)
        {
            return _levelGrid[entityView.GridPositionX, entityView.GridPositionY].ToString();
        }
    }
}