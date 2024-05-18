using System.Collections.Generic;
using App.Scripts.External.Localisation.Config;
using App.Scripts.Scenes.MainMenuScene.LocaleView;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.Factories.ItemView
{
    public class LocaleItemViewFactory : IFactory<List<LocaleItemView>>
    {
        private readonly DiContainer _diContainer;
        private readonly LocaleProvider _localeProvider;
        private readonly ILocaleItemView _prefab;
        private const string LocaleTokenPostfix = "_itemView";

        public LocaleItemViewFactory(DiContainer diContainer, LocaleProvider localeProvider, ILocaleItemView prefab)
        {
            _diContainer = diContainer;
            _localeProvider = localeProvider;
            _prefab = prefab;
        }
        
        public List<LocaleItemView> Create()
        {
            List<LocaleItemView> items = new();

            foreach (LocaleConfig config in _localeProvider.Configs)
            {
                LocaleItemView view = _diContainer
                                            .InstantiatePrefab(_prefab.GameObject)
                                            .GetComponent<LocaleItemView>();

                LocaleViewModel model = new();
                model.Sprite      = config.Sprite;
                model.LocaleKey   = config.Key;
                model.LocaleToken = $"{config.Key}{LocaleTokenPostfix}";

                view.SetModel(model);
                
                items.Add(view);
            }

            return items;
        }
    }
}