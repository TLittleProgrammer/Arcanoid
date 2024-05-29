using System.Collections.Generic;
using System.Linq;
using App.Scripts.External.Grid;
using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using App.Scripts.Scenes.GameScene.Features.Entities.AssetManagement;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer;
using App.Scripts.Scenes.GameScene.Features.Entities.TopSprites;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Grid;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Levels.General.View
{
    public class LevelViewUpdater : ILevelViewUpdater
    {
        private readonly EntityProvider _entityProvider;
        private readonly OnTopSprites.Factory _spritesFactory;
        private readonly IEntityDestroyable _entityDestroyable;
        private readonly IEntityViewService _entityViewService;

        private Grid<int> _levelGrid;
        private Grid<GridItemData> _levelGridItemData = new(Vector2Int.zero);

        public LevelViewUpdater(
            EntityProvider entityProvider,
            OnTopSprites.Factory spritesFactory,
            IEntityDestroyable entityDestroyable,
            IEntityViewService entityViewService)
        {
            _entityProvider = entityProvider;
            _spritesFactory = spritesFactory;
            _entityDestroyable = entityDestroyable;
            _entityViewService = entityViewService;
        }

        public Grid<GridItemData> LevelGridItemData => _levelGridItemData;

        public void SetGrid(Grid<int> grid, List<IEntityView> entityViews)
        {
            _levelGrid = grid;

            InitializeGridItemData(entityViews);
        }

        private void InitializeGridItemData(List<IEntityView> entityViews)
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
                    _levelGridItemData[i, j].BoostTypeId   = entityStage.BoostTypeId;
                    _levelGridItemData[i, j].Damage        = entityStage.Damage;
                    _levelGridItemData[i, j].CanGetDamage  = entityStage.ICanGetDamage;

                    IEntityView entityView = entityViews.First(x => x.GridPositionX == i && x.GridPositionY == j);
                    
                    if (IsActiveBoost(entityView, entityStage))
                    {
                        _entityViewService.AddBoostSprite(entityView, _levelGridItemData[i, j], entityView.BoostTypeId);
                    }
                }
            }
        }

        private static bool IsActiveBoost(IEntityView entityView, EntityStage entityStage)
        {
            return entityView.BoostTypeId is not BoostTypeId.Bomb &&
                   entityView.BoostTypeId is not  BoostTypeId.None &&
                   entityView.BoostTypeId is not  BoostTypeId.HorizontalBomb &&
                   entityView.BoostTypeId is not  BoostTypeId.VerticalBomb &&
                   entityView.BoostTypeId is not  BoostTypeId.CaptiveBall &&
                   entityStage.BoostTypeId is not BoostTypeId.ChainBomb;
        }

        public void UpdateVisual(IEntityView entityView, int damage)
        {
            EntityStage entityStage = GetEntityStage(entityView);

            if (entityStage.ICanGetDamage is false)
                return;
            
            GridItemData itemData = _levelGridItemData[entityView.GridPositionX, entityView.GridPositionY];
            itemData.CurrentHealth -= damage;

            if (ReturnIfCurrentHealthIsEqualsOrLessZero(entityView, itemData))
            {
                return;
            }
            
            _entityViewService.TryAddOnTopSprite(entityView, entityStage, itemData);
        }

        private bool ReturnIfCurrentHealthIsEqualsOrLessZero(IEntityView entityView, GridItemData itemData)
        {
            if (itemData.CurrentHealth <= 0)
            {
                _entityDestroyable.Destroy(itemData, entityView);
                return true;
            }

            return false;
        }

        public EntityStage GetEntityStage(IEntityView entityView)
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