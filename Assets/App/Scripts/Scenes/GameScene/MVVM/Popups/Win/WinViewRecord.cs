using UnityEngine;

namespace App.Scripts.Scenes.GameScene.MVVM.Popups.Win
{
    public record WinViewRecord
    {
        public Sprite Sprite;
        public Sprite TopGalacticSprite;
        public string Token;
        public int CurrentLevel;
        public int AllLevelsCount;

        public WinViewRecord(Sprite sprite, Sprite topGalacticSprite, string token, int currentLevel, int allLevelsCount)
        {
            Sprite = sprite;
            TopGalacticSprite = topGalacticSprite;
            Token = token;
            CurrentLevel = currentLevel;
            AllLevelsCount = allLevelsCount;
        }
    }
}