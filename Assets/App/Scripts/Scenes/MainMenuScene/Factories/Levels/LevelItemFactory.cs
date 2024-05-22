using System.Linq;
using App.Scripts.External.Components;
using App.Scripts.External.GameStateMachine;
using App.Scripts.External.UserData;
using App.Scripts.General.Constants;
using App.Scripts.General.Energy;
using App.Scripts.General.LevelPackInfoService;
using App.Scripts.General.Levels;
using App.Scripts.General.States;
using App.Scripts.General.UserData.Levels.Data;
using App.Scripts.Scenes.MainMenuScene.Constants;
using App.Scripts.Scenes.MainMenuScene.LevelPacks;
using App.Scripts.Scenes.MainMenuScene.LevelPacks.Configs;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.Factories.Levels
{
    public class LevelItemFactory : IFactory<int, LevelPack, ILevelItemView>
    {
        private readonly ILevelItemView _prefab;
        private readonly ITransformable _prefabParent;
        private readonly DiContainer _diContainer;

        public LevelItemFactory(
            ILevelItemView prefab,
            ITransformable prefabParent,
            DiContainer diContainer)
        {
            _prefab = prefab;
            _prefabParent = prefabParent;
            _diContainer = diContainer;
        }
        
        public ILevelItemView Create(int packIndex, LevelPack levelPack)
        {
            ILevelItemView levelItemView = _diContainer.InstantiatePrefab(_prefab.GameObject, _prefabParent.Transform).GetComponent<ILevelItemView>();

            return levelItemView;
        }
    }
}