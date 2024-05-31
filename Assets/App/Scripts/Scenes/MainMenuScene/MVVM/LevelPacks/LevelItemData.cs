using App.Scripts.General.Levels;

namespace App.Scripts.Scenes.MainMenuScene.LevelPacks.MonoBehaviours
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