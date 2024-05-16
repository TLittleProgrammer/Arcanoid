using App.Scripts.General.Levels;
using App.Scripts.Scenes.MainMenuScene.LevelPacks.Configs;

namespace App.Scripts.Scenes.MainMenuScene.LevelPacks
{
    public record LevelModel
    {
        public LevelItemViewData LevelViewData;
        public LevelPack LevelPack;
        public VisualTypeId VisualTypeId;
        public int PassedLevels;
    }
}