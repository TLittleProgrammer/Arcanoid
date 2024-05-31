using App.Scripts.Scenes.MainMenuScene.LevelPacks;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.Factories.Levels
{
    public class LevelItemFactory : IFactory<ILevelItemView>
    {
        private readonly ILevelItemView _prefab;
        private readonly DiContainer _diContainer;

        public LevelItemFactory(
            ILevelItemView prefab,
            DiContainer diContainer)
        {
            _prefab = prefab;
            _diContainer = diContainer;
        }
        
        public ILevelItemView Create()
        {
            ILevelItemView levelItemView = _diContainer.InstantiatePrefab(_prefab.GameObject).GetComponent<ILevelItemView>();

            return levelItemView;
        }
    }
}