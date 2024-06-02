using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Levels.LevelProgress
{
    public record LevelPackInfoRecord
    {
        public Sprite GalacticIconSprite;
        public int CurrentLevelIndex;
        public int AllLevelsCountFromPack;
        public int TargetScore;
    }
}   