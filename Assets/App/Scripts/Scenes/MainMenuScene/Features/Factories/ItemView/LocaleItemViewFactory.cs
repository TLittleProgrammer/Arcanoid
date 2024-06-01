using App.Scripts.Scenes.MainMenuScene.LocaleView;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.Factories.ItemView
{
    public class LocaleItemViewFactory : IFactory<LocaleItemView>
    {
        private readonly DiContainer _diContainer;
        private readonly ILocaleItemView _prefab;

        public LocaleItemViewFactory(DiContainer diContainer, ILocaleItemView prefab)
        {
            _diContainer = diContainer;
            _prefab = prefab;
        }
        
        public LocaleItemView Create()
        {
            return _diContainer
                .InstantiatePrefab(_prefab.GameObject)
                .GetComponent<LocaleItemView>();
        }
    }
}