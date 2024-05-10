using App.Scripts.Scenes.GameScene.Components;
using App.Scripts.Scenes.GameScene.Containers;
using App.Scripts.Scenes.GameScene.Entities;
using App.Scripts.Scenes.GameScene.Grid;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Levels.Load
{
    public sealed class LevelLoader : ILevelLoader
    {
        private readonly IGridPositionResolver _gridPositionResolver;
        private readonly IEntityView.Factory _entityFactory;
        private readonly IContainer<IBoxColliderable2D> _entityViewContainer;
        private LevelData _levelData;

        public LevelLoader(IGridPositionResolver gridPositionResolver, IEntityView.Factory entityFactory, IContainer<IBoxColliderable2D> entityViewContainer)
        {
            _gridPositionResolver = gridPositionResolver;
            _entityFactory = entityFactory;
            _entityViewContainer = entityViewContainer;
        }

        public void LoadLevel(LevelData levelData)
        {
            for (int i = 0; i < levelData.Grid.GetLength(1); i++)
            {
                for (int j = 0; j < levelData.Grid.GetLength(0); j++)
                {
                    Vector2 targetPosition = _gridPositionResolver.GetCurrentGridPosition();

                    int index = levelData.Grid[j, i];

                    IEntityView spawnedBlock = _entityFactory.Create(index.ToString());
                
                    spawnedBlock.Position = targetPosition;
                    spawnedBlock.Scale = _gridPositionResolver.GetCellSize();
                    
                    _entityViewContainer.AddItem(spawnedBlock);
                }
            }
        }
    }
}