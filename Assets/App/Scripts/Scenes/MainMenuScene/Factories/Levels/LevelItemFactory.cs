using System.Linq;
using App.Scripts.External.Components;
using App.Scripts.External.GameStateMachine;
using App.Scripts.External.UserData;
using App.Scripts.General.Constants;
using App.Scripts.General.Levels;
using App.Scripts.General.States;
using App.Scripts.General.UserData.Levels;
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
        private readonly IUserDataContainer _userDataContainer;
        private readonly LevelItemViewByTypeProvider _levelItemViewByTypeProvider;
        private readonly ILevelPackTransferData _levelPackTransferData;
        private readonly IStateMachine _stateMachine;
        private readonly LevelPackProvider _levelPackProvider;
        private readonly DiContainer _diContainer;

        public LevelItemFactory(
            ILevelItemView prefab,
            ITransformable prefabParent,
            IUserDataContainer userDataContainer,
            LevelItemViewByTypeProvider levelItemViewByTypeProvider,
            ILevelPackTransferData levelPackTransferData,
            IStateMachine stateMachine,
            LevelPackProvider levelPackProvider,
            DiContainer diContainer)
        {
            _prefab = prefab;
            _prefabParent = prefabParent;
            _userDataContainer = userDataContainer;
            _levelItemViewByTypeProvider = levelItemViewByTypeProvider;
            _levelPackTransferData = levelPackTransferData;
            _stateMachine = stateMachine;
            _levelPackProvider = levelPackProvider;
            _diContainer = diContainer;
        }
        
        public ILevelItemView Create(int packIndex, LevelPack levelPack)
        {
            ILevelItemView levelItemView = _diContainer.InstantiatePrefab(_prefab.GameObject, _prefabParent.Transform).GetComponent<ILevelItemView>();

            LevelPackProgressDictionary packProgress = (LevelPackProgressDictionary)_userDataContainer.GetData<LevelPackProgressDictionary>();
            
            VisualTypeId visualType = GetVisualType(packIndex, levelPack, packProgress);
            
            ChangeVisual(packIndex, levelItemView, levelPack, visualType, packProgress);
            SubsribeOnClickIfNeed(levelItemView, packIndex, levelPack, visualType, packProgress);
            
            return levelItemView;
        }

        private void SubsribeOnClickIfNeed(ILevelItemView levelItemView, int packIndex, LevelPack levelPack, VisualTypeId visualType, LevelPackProgressDictionary packProgress)
        {
            int targetLevelIndex;
            if (visualType is not VisualTypeId.NotOpened)
            {
                if (visualType is VisualTypeId.Passed)
                {
                    targetLevelIndex = 0;
                }
                else
                {
                    targetLevelIndex = packProgress.ContainsKey(packIndex)
                        ? packProgress[packIndex].PassedLevels
                        : 0;
                }
                
                levelItemView.Clicked += () =>
                {
                    _levelPackTransferData.NeedLoadLevel = true;
                    _levelPackTransferData.LevelIndex = targetLevelIndex;
                    _levelPackTransferData.LevelPack = levelPack;
                    _levelPackTransferData.PackIndex = packIndex;
                    
                    _stateMachine.Enter<LoadingSceneState, string, bool>(SceneNaming.Game, false);
                };
            }
        }

        private void ChangeVisual(int packIndex, ILevelItemView levelItemView, LevelPack levelPack, VisualTypeId visualType, LevelPackProgressDictionary packProgress)
        {
            LevelItemViewData levelItemViewData = _levelItemViewByTypeProvider.Views[visualType];
            int passedLevels = packProgress.ContainsKey(packIndex) ? packProgress[packIndex].PassedLevels : 0;
                
            levelItemView.UpdateVisual(new()
            {
                LevelViewData = levelItemViewData,
                LevelPack = levelPack,
                VisualTypeId = visualType,
                PassedLevels = passedLevels,
                LocaleKey = visualType is VisualTypeId.NotOpened ? MainSceneLocaleConstants.GalacticNotFoundKey : levelPack.LocaleKey
            });
        }

        private VisualTypeId GetVisualType(int packIndex, LevelPack levelPack, LevelPackProgressDictionary packProgress)
        {
            if (packIndex == 0 && packProgress.Count == 0)
            {
                packProgress.Add(0, new());
                return VisualTypeId.InProgress;
            }

            var lastOpenedPack = packProgress.Last();

            if (packIndex == lastOpenedPack.Key + 1 &&
                lastOpenedPack.Value.PassedLevels >= _levelPackProvider.LevelPacks[lastOpenedPack.Key].Levels.Count)
            {
                return VisualTypeId.InProgress;
            }
            
            if (!packProgress.ContainsKey(packIndex))
            {
                return VisualTypeId.NotOpened;
            }

            if (packProgress[packIndex].PassedLevels >= levelPack.Levels.Count)
            {
                return VisualTypeId.Passed;
            }

            return VisualTypeId.InProgress;
        }
    }
}