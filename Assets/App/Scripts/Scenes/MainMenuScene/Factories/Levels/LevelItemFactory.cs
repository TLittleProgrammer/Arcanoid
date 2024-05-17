using App.Scripts.External.Components;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Constants;
using App.Scripts.General.Levels;
using App.Scripts.General.States;
using App.Scripts.General.UserData.Data;
using App.Scripts.Scenes.MainMenuScene.LevelPacks;
using App.Scripts.Scenes.MainMenuScene.LevelPacks.Configs;
using TMPro;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.Factories.Levels
{
    public class LevelItemFactory : IFactory<int, LevelPack, ILevelItemView>
    {
        private readonly ILevelItemView _prefab;
        private readonly ITransformable _prefabParent;
        private readonly LevelPackProgressDictionary _levelPackProgressDictionary;
        private readonly LevelItemViewByTypeProvider _levelItemViewByTypeProvider;
        private readonly ILevelPackTransferData _levelPackTransferData;
        private readonly IStateMachine _stateMachine;

        public LevelItemFactory(
            ILevelItemView prefab,
            ITransformable prefabParent,
            LevelPackProgressDictionary levelPackProgressDictionary,
            LevelItemViewByTypeProvider levelItemViewByTypeProvider,
            ILevelPackTransferData levelPackTransferData,
            IStateMachine stateMachine)
        {
            _prefab = prefab;
            _prefabParent = prefabParent;
            _levelPackProgressDictionary = levelPackProgressDictionary;
            _levelItemViewByTypeProvider = levelItemViewByTypeProvider;
            _levelPackTransferData = levelPackTransferData;
            _stateMachine = stateMachine;
        }
        
        public ILevelItemView Create(int packIndex, LevelPack levelPack)
        {
            ILevelItemView levelItemView = Object.Instantiate(_prefab.GameObject, _prefabParent.Transform).GetComponent<ILevelItemView>();
            VisualTypeId visualType = GetVisualType(packIndex, levelPack);
            
            ChangeVisual(packIndex, levelItemView, levelPack, visualType);
            SubsribeOnClickIfNeed(levelItemView, packIndex, levelPack, visualType);
            
            return levelItemView;
        }

        private void SubsribeOnClickIfNeed(ILevelItemView levelItemView, int packIndex, LevelPack levelPack, VisualTypeId visualType)
        {
            if (visualType is not VisualTypeId.NotOpened)
            {
                levelItemView.Clicked += () =>
                {
                    _levelPackTransferData.NeedLoadLevel = true;
                    _levelPackTransferData.LevelIndex = _levelPackProgressDictionary[packIndex].PassedLevels;
                    _levelPackTransferData.LevelPack = levelPack;
                    _levelPackTransferData.LevelIndex = packIndex;
                    _levelPackTransferData.LevelPackProgress = 0f;
                    
                    _stateMachine.Enter<LoadingSceneState, string, bool>(SceneNaming.Game, false);
                };
            }
        }

        private void ChangeVisual(int packIndex, ILevelItemView levelItemView, LevelPack levelPack, VisualTypeId visualType)
        {
            LevelItemViewData levelItemViewData = _levelItemViewByTypeProvider.Views[visualType];
            int passedLevels = _levelPackProgressDictionary.ContainsKey(packIndex) ? _levelPackProgressDictionary[packIndex].PassedLevels : 0;
                
            levelItemView.UpdateVisual(new()
            {
                LevelViewData = levelItemViewData,
                LevelPack = levelPack,
                VisualTypeId = visualType,
                PassedLevels = passedLevels
            });
        }

        private VisualTypeId GetVisualType(int packIndex, LevelPack levelPack)
        {
            if (packIndex == 0 && _levelPackProgressDictionary.Count == 0)
            {
                _levelPackProgressDictionary.Add(0, new());
                return VisualTypeId.InProgress;
            }
            
            if (!_levelPackProgressDictionary.ContainsKey(packIndex))
            {
                return VisualTypeId.NotOpened;
            }

            if (_levelPackProgressDictionary[packIndex].PassedLevels == levelPack.Levels.Count)
            {
                return VisualTypeId.Passed;
            }

            return VisualTypeId.InProgress;
        }

        public ILevelItemView Create()
        {
            throw new System.NotImplementedException();
        }
    }
}