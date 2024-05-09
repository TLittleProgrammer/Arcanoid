﻿using App.Scripts.Scenes.GameScene.Entities;
using App.Scripts.Scenes.GameScene.Grid;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Levels.Load
{
    public sealed class LevelLoader : ILevelLoader
    {
        private readonly IGridPositionResolver _gridPositionResolver;
        private readonly IEntityView.Factory _entityFactory;
        private LevelData _levelData;

        public LevelLoader(IGridPositionResolver gridPositionResolver, IEntityView.Factory entityFactory)
        {
            _gridPositionResolver = gridPositionResolver;
            _entityFactory = entityFactory;
        }

        public void LoadLevel(LevelData levelData)
        {
            int blocksCounter = levelData.GridSize.x * levelData.GridSize.y;

            for (int i = 0; i < levelData.Grid.GetLength(1); i++)
            {
                for (int j = 0; j < levelData.Grid.GetLength(0); j++)
                {
                    Vector2 targetPosition = _gridPositionResolver.GetCurrentGridPosition();

                    int index = levelData.Grid[j, i];
                    Debug.Log($"Position ({i}, {j}): " + levelData.Grid[j, i]);

                    IEntityView spawnedBlock = _entityFactory.Create(index.ToString());
                
                    spawnedBlock.Position = targetPosition;
                    spawnedBlock.Scale = _gridPositionResolver.GetCellSize();
                }
            }
        }

        private static int GetMatrixValue(LevelData levelData, int i)
        {
            int x = i / levelData.GridSize.x;
            int y = i % levelData.GridSize.y;
            
            return levelData.Grid[y, x];
        }
    }
}