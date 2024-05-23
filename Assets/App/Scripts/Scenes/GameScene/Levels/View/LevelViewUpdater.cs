using App.Scripts.External.Grid;
using App.Scripts.Scenes.GameScene.Entities;
using App.Scripts.Scenes.GameScene.Levels.AssetManagement;
using App.Scripts.Scenes.GameScene.Levels.Data;
using App.Scripts.Scenes.GameScene.Levels.ItemsDestroyer;
using App.Scripts.Scenes.GameScene.TopSprites;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Levels.View
{
    public class LevelViewUpdater : ILevelViewUpdater
    {
        private readonly EntityProvider _entityProvider;
        private readonly OnTopSprites.Factory _spritesFactory;
        private readonly IItemsDestroyable _itemsDestroyable;
        private readonly IItemViewDamageService _itemViewDamageService;

        private Grid<int> _levelGrid;
        private Grid<GridItemData> _levelGridItemData = new(Vector2Int.zero);

        public LevelViewUpdater(
            EntityProvider entityProvider,
            OnTopSprites.Factory spritesFactory,
            IItemsDestroyable itemsDestroyable,
            IItemViewDamageService itemViewDamageService)
        {
            _entityProvider = entityProvider;
            _spritesFactory = spritesFactory;
            _itemsDestroyable = itemsDestroyable;
            _itemViewDamageService = itemViewDamageService;
        }

        public Grid<GridItemData> LevelGridItemData => _levelGridItemData;

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
                    _levelGridItemData[i, j].BoostTypeId   = entityStage.BoostTypeId;
                    _levelGridItemData[i, j].Damage        = entityStage.Damage;
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

            if (ReturnIfCurrentHealthIsEqualsOrLessZero(entityView, itemData))
            {
                return;
            }
            
            _itemViewDamageService.TryAddOnTopSprite(entityView, entityStage, itemData);
        }

        private bool ReturnIfCurrentHealthIsEqualsOrLessZero(IEntityView entityView, GridItemData itemData)
        {
            if (itemData.CurrentHealth <= 0)
            {
                _itemsDestroyable.Destroy(itemData, entityView);
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