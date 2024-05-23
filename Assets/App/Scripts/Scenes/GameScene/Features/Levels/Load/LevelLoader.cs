using System.Collections.Generic;
using App.Scripts.External.Grid;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Grid;
using App.Scripts.Scenes.GameScene.Features.Levels.View;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Levels.Load
{
    public sealed class LevelLoader : ILevelLoader
    {
        private readonly IGridPositionResolver _gridPositionResolver;
        private readonly IEntityView.Factory _entityFactory;
        private readonly ILevelViewUpdater _levelViewUpdater;
        
        private LevelData _previousLevelData;
        private List<IEntityView> _entityViews;
        public List<IEntityView> Entities => _entityViews;

        public LevelLoader(
            IGridPositionResolver gridPositionResolver,
            IEntityView.Factory entityFactory,
            ILevelViewUpdater levelViewUpdater)
        {
            _gridPositionResolver = gridPositionResolver;
            _entityFactory = entityFactory;
            _levelViewUpdater = levelViewUpdater;
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
                    Vector2 targetPosition = _gridPositionResolver.GetCurrentGridPosition();

                    int index = levelData.Grid[j, i];

                    if (index <= 0)
                    {
                        continue;
                    }
                    
                    levelGrid[j, i] = index;

                    IEntityView spawnedBlock = _entityFactory.Create(index.ToString());
                
                    spawnedBlock.Position = targetPosition;
                    spawnedBlock.Scale = _gridPositionResolver.GetCellSize();
                    spawnedBlock.GridPositionX = j;
                    spawnedBlock.GridPositionY = i;
                    spawnedBlock.BoxCollider2D.enabled = true;
                    
                    _entityViews.Add(spawnedBlock);
                }
            }

            _levelViewUpdater.SetGrid(levelGrid);
        }

        public void Restart()
        {
            LoadLevel(_previousLevelData);
        }
    }
}