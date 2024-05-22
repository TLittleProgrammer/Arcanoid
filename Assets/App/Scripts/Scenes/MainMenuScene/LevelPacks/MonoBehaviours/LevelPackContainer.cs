using System.Linq;
using App.Scripts.External.GameStateMachine;
using App.Scripts.External.UserData;
using App.Scripts.General.Constants;
using App.Scripts.General.Energy;
using App.Scripts.General.LevelPackInfoService;
using App.Scripts.General.Levels;
using App.Scripts.General.States;
using App.Scripts.General.UserData.Levels.Data;
using App.Scripts.Scenes.MainMenuScene.Constants;
using App.Scripts.Scenes.MainMenuScene.LevelPacks.Configs;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.LevelPacks.MonoBehaviours
{
    public class LevelPackContainer : MonoBehaviour
    {
        private LevelItemViewByTypeProvider _levelItemViewByTypeProvider;
        private IStateMachine _stateMachine;
        private LevelPackProvider _levelPackProvider;
        private ILevelPackInfoService _levelPackInfoService;
        private IEnergyService _energyService;

        [Inject]
        private void Construct(
            LevelPackProvider levelPackProvider,
            ILevelItemView.Factory levelItemFactory,
            IUserDataContainer userDataContainer,
            LevelItemViewByTypeProvider levelItemViewByTypeProvider,
            IStateMachine stateMachine,
            ILevelPackInfoService levelPackInfoService,
            IEnergyService energyService)
        {
            _energyService = energyService;
            _levelItemViewByTypeProvider = levelItemViewByTypeProvider;
            _levelPackProvider = levelPackProvider;
            _levelPackInfoService = levelPackInfoService;
            _stateMachine = stateMachine;
            
            CreateAllLevelPacks(levelItemFactory, (LevelPackProgressDictionary)userDataContainer.GetData<LevelPackProgressDictionary>());
        }

        private void CreateAllLevelPacks(ILevelItemView.Factory levelItemFactory, LevelPackProgressDictionary levelPackProgressDictionary)
        {
            for (int i = 0; i < _levelPackProvider.LevelPacks.Count; i++)
            {
                ILevelItemView levelItemView = levelItemFactory.Create(i, _levelPackProvider.LevelPacks[i]);
                
                VisualTypeId visualType = GetVisualType(i, _levelPackProvider.LevelPacks[i], levelPackProgressDictionary);
            
                ChangeVisual(i, levelItemView, _levelPackProvider.LevelPacks[i], visualType, levelPackProgressDictionary);
                SubsribeOnClickIfNeed(levelItemView, i, _levelPackProvider.LevelPacks[i], visualType, levelPackProgressDictionary);
            }
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
                    _levelPackInfoService.SetData(new LevelPackTransferData()
                    {
                        NeedLoadLevel = true,
                        LevelIndex = targetLevelIndex,
                        LevelPack = levelPack,
                        PackIndex = packIndex
                    });
                  
                    _energyService.Dispose();
                    
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