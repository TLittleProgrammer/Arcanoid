﻿namespace App.Scripts.Scenes.GameScene.Features.Entities
{
    public enum BoostTypeId
    {
        None = 0,
        
        Bomb = 1,
        BallAcceleration = 2,
        BallSlowdown     = 3,
        PlayerShapeAddSize   = 4,
        PlayerShapeMinusSize = 5,
        
        PlayerShapeAddSpeed   = 6,
        PlayerShapeMinusSpeed = 7,
        
        AddHealth   = 8,
        MinusHealth = 9,
    }
}