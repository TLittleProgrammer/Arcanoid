using System;
using Unity.Mathematics;

namespace App.Scripts.Scenes.GameScene.Levels.ItemsDestroyer.DestroyServices
{
    public enum Direction
    {
        None = 0,
        
        Right = 0,
        Up = 1,
        Left = 2,
        Down = 3,
        UpRight = 4,
        UpLeft = 5,
        DownLeft = 6,
        DownRight = 7
    }
    
    public static class DirectionExtensions
    {
        public static int2 ToVector(this Direction direction)
        {
            return direction switch
            {
                Direction.Right     => new int2(1, 0),
                Direction.Up        => new int2(0, 1),
                Direction.Left      => new int2(-1, 0),
                Direction.Down      => new int2(0, -1),
                Direction.UpRight   => new int2(1, 1),
                Direction.UpLeft    => new int2(-1, 1),
                Direction.DownLeft  => new int2(-1, -1),
                Direction.DownRight => new int2(1, -1),
                
                _ => throw new ArgumentException($"Направление {direction} не может быть преобразовано в int2")
            };
        }
    }
}