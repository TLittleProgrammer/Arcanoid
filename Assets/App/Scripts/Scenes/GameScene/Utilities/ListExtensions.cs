using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using Unity.Mathematics;

namespace App.Scripts.Scenes.GameScene.Utilities
{
    public static class ListExtensions
    {
        public static IEntityView GetByCoordinates(this List<IEntityView> list, int2 coordinates)
        {
            return list.First(x => x.GridPositionX == coordinates.x && x.GridPositionY == coordinates.y);
        }
    }
}