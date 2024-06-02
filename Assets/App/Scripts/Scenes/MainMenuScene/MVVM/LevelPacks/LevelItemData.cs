using App.Scripts.General.Levels;
using App.Scripts.Scenes.MainMenuScene.Features.LevelPacks;

namespace App.Scripts.Scenes.MainMenuScene.MVVM.LevelPacks
{
    public record LevelItemData
    {
        public int PackIndex;
        public LevelPack LevelPack;
        public ILevelItemView LevelItemView;

        public LevelItemData(LevelPack levelPack, ILevelItemView levelItemView, int packIndex)
        {
            LevelPack = levelPack;
            LevelItemView = levelItemView;
            PackIndex = packIndex;
        }
    }
}