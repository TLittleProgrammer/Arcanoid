using System.Collections.Generic;
using System.Drawing;
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
            
            while (PointOnMap(initialPoint))
            {
                result.Add(initialPoint);
                initialPoint += direction.ToVector();
            }


            return result.ToArray();
        }

        protected bool PointOnMap(int2 point)
        {
            return point.x >= 0 &&
                   point.y >= 0 &&
                   point.x < LevelViewUpdater.LevelGridItemData.Width &&
                   point.y < LevelViewUpdater.LevelGridItemData.Height;
        }
        
        protected List<int2> GetAroundPoints(int2 initialPoint)
        {
            return new()
            {
                initialPoint + Direction.Up.ToVector(),
                initialPoint + Direction.Right.ToVector(),
                initialPoint + Direction.Down.ToVector(),
                initialPoint + Direction.Left.ToVector(),
            };
        }
    }
}