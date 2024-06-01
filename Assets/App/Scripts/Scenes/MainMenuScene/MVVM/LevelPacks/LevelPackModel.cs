using System.Collections.Generic;
using App.Scripts.General.Components;
using App.Scripts.General.Levels;

namespace App.Scripts.Scenes.MainMenuScene.LevelPacks.MonoBehaviours
{
    public class LevelPackModel : IModel
    {
        private readonly LevelPackProvider _levelPackProvider;
        private readonly ILevelItemView.Factory _levelItemFactory;

        public LevelPackModel(
            LevelPackProvider levelPackProvider,
            ILevelItemView.Factory levelItemFactory)
        {
            _levelPackProvider = levelPackProvider;
            _levelItemFactory = levelItemFactory;
        }

        public List<LevelItemData> GetLevelItemDatas()
        {
            List<LevelItemData> itemDatas = new();
            int packCounters = 0;
            
            foreach (LevelPack levelPack in _levelPackProvider.LevelPacks)
            {
                ILevelItemView levelItemView = _levelItemFactory.Create();
                
                itemDatas.Add(new(levelPack, levelItemView, packCounters++));
            }

            return itemDatas;
        }
    }
}