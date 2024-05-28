using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.GameScene.Features.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.EntryPoint.Utilities
{
    public static class ListExtensions
    {
        public static IEntityView GetByCoordinates(this List<IEntityView> list, int2 coordinates)
        {
            return list.First(x => x.GridPositionX == coordinates.x && x.GridPositionY == coordinates.y);
        }
    }
}