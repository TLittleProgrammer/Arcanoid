using UnityEngine;

namespace App.Scripts.Scenes.GameScene.LevelProgress
{
    public record LevelPackInfoRecord
    {
        public Sprite Sprite;
        public int CurrentLevelIndex;
        public int AllLevelsCountFromPack;
        public int TargetScore;
    }
}   