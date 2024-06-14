using System.Collections.Generic;
using System.Linq;
using App.Scripts.External.Grid;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Grid;
using App.Scripts.Scenes.GameScene.Features.Levels.General;
using App.Scripts.Scenes.GameScene.Features.Levels.General.View;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress.Data;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Levels.Loading.Loader
{
    public sealed class LevelLoader : ILevelLoader, ILevelProgressSavable, IInitializeByLevelProgress
    {
        private readonly IGridPositionResolver _gridPositionResolver;
        private readonly IEntityView.Factory _entityFactory;
        private readonly ILevelViewUpdater _levelViewUpdater;
        private readonly EntityView.Pool _entityViewPool;
        private readonly ILevelDataChooser _levelDataChooser;
        private readonly IGridDataService _gridDataService;

        private LevelData _previousLevelData;
        private List<IEntityView> _entityViews;
        public List<IEntityView> Entities => _entityViews;

        public LevelLoader(
            IGridPositionResolver gridPositionResolver,
            IEntityView.Factory entityFactory,
            ILevelViewUpdater levelViewUpdater,
            EntityView.Pool entityViewPool,
            ILevelDataChooser levelDataChooser,
            IGridDataService gridDataService)
        {
            _gridPositionResolver = gridPositionResolver;
            _entityFactory = entityFactory;
            _levelViewUpdater = levelViewUpdater;
            _entityViewPool = entityViewPool;
            _levelDataChooser = levelDataChooser;
            _gridDataService = gridDataService;
            _entityViews = new();
        }

        public void LoadLevel(LevelData levelData)
        {
            Grid<int> levelGrid = new Grid<int>(new(levelData.GridSize.x, levelData.GridSize.y));
            _previousLevelData = levelData;
            
            _entityViews.Clear();

            for (int i = 0; i < levelData.Grid.GetLength(1); i++)
            {
                for (int j = 0; j < levelData.Grid.GetLength(0); j++)
                {
                    Vector2 targetPosition = _gridPositionResolver.GetPositionByCoordinates(j, i);

                    int index = levelData.Grid[j, i];

                    if (index <= 0)
                    {
                        continue;
                    }
                    
                    levelGrid[j, i] = index;
                    SpawnBlock(targetPosition, j, i, index, Vector3.zero);
                }
            }

            _levelViewUpdater.SetGrid(levelGrid, _entityViews);
        }

        public void Restart()
        {
            if (_previousLevelData is null)
            {
                _previousLevelData = _levelDataChooser.GetLevelData();
            }

            foreach (IEntityView view in _entityViews)
            {
                if (view.GameObject.activeSelf)
                {
                    _entityViewPool.Despawn(view as EntityView);
                }
            }
            LoadLevel(_previousLevelData);
        }

        public void SaveProgress(LevelDataProgress levelDataProgress)
        {
            levelDataProgress.EntityDatas = new();
            
            foreach (IEntityView entity in Entities)
            {
                EntitySaveData entityData = new EntitySaveData();

                entityData.EntityId = entity.EntityId;
                entityData.GridPositionX = entity.GridPositionX;
                entityData.GridPositionY = entity.GridPositionY;

                levelDataProgress.EntityDatas.Add(entityData);
            }
        }

        public void LoadProgress(LevelDataProgress levelDataProgress)
        {
            _entityViews = new();
            
            SpawnAllViews(levelDataProgress);
            UpdateVisualsBeforeLoading(levelDataProgress);
        }

        private void SpawnAllViews(LevelDataProgress levelDataProgress)
        {
            Grid<int> levelGrid = new Grid<int>(new(levelDataProgress.GridSizeX, levelDataProgress.GridSizeY));
            
            foreach (EntitySaveData entityData in levelDataProgress.EntityDatas)
            {
                if (IsEntityDestroyed(levelDataProgress, entityData))
                {
                    continue;
                }

                Vector2 targetPosition = _gridPositionResolver.GetPositionByCoordinates(entityData.GridPositionX, entityData.GridPositionY);

                levelGrid[entityData.GridPositionX, entityData.GridPositionY] = entityData.EntityId;
                SpawnBlock(targetPosition, entityData.GridPositionX, entityData.GridPositionY, entityData.EntityId, _gridDataService.CellSize);
            }
            
            _levelViewUpdater.SetGrid(levelGrid, _entityViews);
        }

        private void UpdateVisualsBeforeLoading(LevelDataProgress levelDataProgress)
        {
            foreach (IEntityView view in _entityViews)
            {
                SaveGridItemData save = levelDataProgress.EntityGridItemsData.First(x => x.GridPositionX == view.GridPositionX && x.GridPositionY == view.GridPositionY);
                
                _levelViewUpdater.FastUpdateVisual(view, save.CurrentHealth);
                _levelViewUpdater.SetHealth(view, save.CurrentHealth);
            }
        }

        private bool IsEntityDestroyed(LevelDataProgress levelDataProgress, EntitySaveData entityData)
        {
            return levelDataProgress.EntityGridItemsData.First(x => x.GridPositionX == entityData.GridPositionX && x.GridPositionY == entityData.GridPositionY).CurrentHealth <= 0;
        }

        private IEntityView SpawnBlock(Vector2 targetPosition, int x, int y, int id, Vector3 scale)
        {
            IEntityView spawnedBlock = _entityFactory.Create(id.ToString());

            spawnedBlock.Position = targetPosition;
            spawnedBlock.Scale = scale;
            spawnedBlock.GridPositionX = x;
            spawnedBlock.GridPositionY = y;
            spawnedBlock.BoxCollider2D.enabled = true;
            spawnedBlock.EntityId = id;
            
            _entityViews.Add(spawnedBlock);

            return spawnedBlock;
        }
    }
}