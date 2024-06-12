using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators;
using App.Scripts.Scenes.GameScene.Features.Boosts.General.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.Helpers;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress.Data;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General
{
    public class BoostsActivator : IBoostsActivator, IInitializeByLevelProgress
    {
        private readonly SimpleDestroyService _simpleDestroyService;
        private readonly IBoostContainer _boostContainer;
        private readonly Dictionary<string, IConcreteBoostActivator> _concreteBoostActivators;

        private float _initialBallSpeed;

        public BoostsActivator(
            SimpleDestroyService simpleDestroyService,
            IBoostContainer boostContainer,
            Dictionary<string, IConcreteBoostActivator> concreteBoostActivators)
        {
            _simpleDestroyService = simpleDestroyService;
            _boostContainer = boostContainer;
            _concreteBoostActivators = concreteBoostActivators;

            _boostContainer.BoostEnded += OnBoostEnded;
        }

        public void Activate(BoostView view)
        {
            string boostId = view.BoostTypeId.ToString();
            
            _concreteBoostActivators[boostId].Activate();

            if (_concreteBoostActivators[boostId].IsTimeableBoost)
            {
                _boostContainer.AddBoost(view.BoostTypeId);
            }
            
            _simpleDestroyService.Destroy(view);
        }

        private void OnBoostEnded(BoostTypeId boostType)
        {
            _concreteBoostActivators[boostType.ToString()].Deactivate();
        }

        public void LoadProgress(LevelDataProgress levelDataProgress)
        {
            foreach (SaveActiveBoostData boostData in levelDataProgress.ActiveBoostDatas)
            {
                _concreteBoostActivators[boostData.BoostTypeId.ToString()].Activate();
            }
        }
    }
}