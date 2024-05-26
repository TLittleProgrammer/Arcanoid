using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Levels.Data;
using App.Scripts.Scenes.GameScene.Features.Levels.View;
using Unity.Mathematics;

namespace App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.DestroyServices
{
    public abstract class DestroyService : IBlockDestroyService
    {
        protected readonly ILevelViewUpdater LevelViewUpdater;

        protected DestroyService(ILevelViewUpdater levelViewUpdater)
        {
            LevelViewUpdater = levelViewUpdater;
        }
        
        public abstract void Destroy(GridItemData gridItemData, IEntityView entityView);
        
        protected int2[] GetAllPointsByDirection(int2 initialPoint, Direction direction)
        {
            List<int2> result = new();

            initialPoint += direction.ToVector();
            
            while (initialPoint.x >= 0 &&
                   initialPoint.y >= 0 &&
                   initialPoint.x < LevelViewUpdater.LevelGridItemData.Width &&
                   initialPoint.y < LevelViewUpdater.LevelGridItemData.Height)
            {
                result.Add(initialPoint);
                initialPoint += direction.ToVector();
            }


            return result.ToArray();
        }
    }
}