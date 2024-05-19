using System.Linq;
using App.Scripts.External.Extensions.ListExtensions;
using App.Scripts.External.Grid;
using App.Scripts.Scenes.GameScene.Ball;
using App.Scripts.Scenes.GameScene.Entities;
using App.Scripts.Scenes.GameScene.LevelProgress;
using App.Scripts.Scenes.GameScene.Levels.AssetManagement;
using App.Scripts.Scenes.GameScene.Levels.Data;
using App.Scripts.Scenes.GameScene.Pools;
using App.Scripts.Scenes.GameScene.TopSprites;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Levels.View
{
    public class LevelViewUpdater : ILevelViewUpdater
    {
        private readonly EntityProvider _entityProvider;
        private readonly IPoolContainer _poolContainer;
        private readonly ILevelProgressService _levelProgressService;
        private readonly IBallSpeedUpdater _ballSpeedUpdater;
        private readonly OnTopSprites.Factory _spritesFactory;

        private Grid<int> _levelGrid;
        private Grid<GridItemData> _levelGridItemData = new(Vector2Int.zero);

        public LevelViewUpdater(
            EntityProvider entityProvider,
            IPoolContainer poolContainer,
            ILevelProgressService levelProgressService,
            IBallSpeedUpdater ballSpeedUpdater,
            OnTopSprites.Factory spritesFactory)
        {
            _entityProvider = entityProvider;
            _poolContainer = poolContainer;
            _levelProgressService = levelProgressService;
            _ballSpeedUpdater = ballSpeedUpdater;
            _spritesFactory = spritesFactory;
        } 

        public void SetGrid(Grid<int> grid)
        {
            _levelGrid = grid;

            InitializeGridItemData();
        }

        private void InitializeGridItemData()
        {
            _levelGridItemData.UpdateMatrix(_levelGrid.Size);

            for (int i = 0; i < _levelGrid.Width; i++)
            {
                for (int j = 0; j < _levelGrid.Height; j++)
                {
                    string index = _levelGrid[i, j].ToString();
                    EntityStage entityStage = _entityProvider.EntityStages[index];

                    _levelGridItemData[i, j] = new();
                    _levelGridItemData[i, j].CurrentHealth = entityStage.MaxHealthCounter;
                }
            }
        }

        public void UpdateVisual(IEntityView entityView)
        {
            EntityStage entityStage = GetEntityStage(entityView);

            if (entityStage.ICanGetDamage is false)
                return;
            
            GridItemData itemData = _levelGridItemData[entityView.GridPositionX, entityView.GridPositionY];
            itemData.CurrentHealth--;

            if (ReturnIfCurrentHealthIsEqualsOrLessZero(entityView, itemData)) return;
            TryAddOnTopSprite(entityView, entityStage, itemData);
        }

        private void TryAddOnTopSprite(IEntityView entityView, EntityStage entityStage, GridItemData itemData)
        {
            HealthSpriteData healthSpriteData = entityStage.AddSpritesOnMainByHp.FirstOrDefault(x => x.Healthes == itemData.CurrentHealth);

            if (healthSpriteData is not null)
            {
                OnTopSprites topSprite = _spritesFactory.Create(entityView);
                topSprite.SetSprite(healthSpriteData.Sprites.GetRandomValue());

                itemData.Sprites.Add(topSprite);
            }
        }

        private bool ReturnIfCurrentHealthIsEqualsOrLessZero(IEntityView entityView, GridItemData itemData)
        {
            if (itemData.CurrentHealth <= 0)
            {
                _levelProgressService.TakeOneStep();

                _poolContainer.RemoveItem(PoolTypeId.EntityView, entityView as EntityView);
                foreach (OnTopSprites sprite in itemData.Sprites)
                {
                    _poolContainer.RemoveItem(PoolTypeId.OnTopSprite, sprite);
                }

                _ballSpeedUpdater.UpdateSpeed();
                return true;
            }

            return false;
        }

        private EntityStage GetEntityStage(IEntityView entityView)
        {
            string index = GetIndexByEntityView(entityView);

            EntityStage entityStage = _entityProvider.EntityStages[index];
            return entityStage;
        }

        private string GetIndexByEntityView(IEntityView entityView)
        {
            return _levelGrid[entityView.GridPositionX, entityView.GridPositionY].ToString();
        }
    }
}