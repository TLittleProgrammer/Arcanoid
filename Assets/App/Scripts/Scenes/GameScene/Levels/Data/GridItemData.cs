using System;
using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.TopSprites;

namespace App.Scripts.Scenes.GameScene.Levels.Data
{
    [Serializable]
    public class GridItemData
    {
        public int CurrentHealth;
        public List<OnTopSprites> Sprites = new();
    }
}