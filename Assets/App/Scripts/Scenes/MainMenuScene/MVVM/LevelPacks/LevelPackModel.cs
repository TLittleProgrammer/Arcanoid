using System.Collections.Generic;
using App.Scripts.General.Components;
using App.Scripts.General.Levels;
using App.Scripts.Scenes.MainMenuScene.Features.LevelPacks;

namespace App.Scripts.Scenes.MainMenuScene.MVVM.LevelPacks
{
    public class LevelPackModel : IModel
    {
        private readonly LevelPackProvider _levelPackProvider;
        private readonly ILevelItemView.Factory _levelItemFactory;
        private List<LevelItemData> _itemDatas;

        public LevelPackModel(LevelPackProvider levelPackProvider, ILevelItemView.Factory levelItemFactory)
        {
            _levelPackProvider = levelPackProvider;
            _levelItemFactory = levelItemFactory;
        }

        public List<LevelItemData> GetLevelItemDatas()
        {
            _itemDatas = new();
            int packCounters = 0;
            
            foreach (LevelPack levelPack in _levelPackProvider.LevelPacks)
            {
                ILevelItemView levelItemView = _levelItemFactory.Create();
                
                _itemDatas.Add(new(levelPack, levelItemView, packCounters++));
            }

            return _itemDatas;
        }

        public LevelItemData GetFirstLevelItemData()
        {
            return _itemDatas[0];
        }
    }
}