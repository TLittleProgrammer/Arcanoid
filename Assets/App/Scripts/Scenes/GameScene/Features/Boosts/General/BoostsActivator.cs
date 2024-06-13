using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators;
using App.Scripts.Scenes.GameScene.Features.Boosts.General.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.Helpers;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress.Data;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General
{
    public class BoostsActivator : IBoostsActivator, IInitializeByLevelProgress
    {
        private readonly IBoostContainer _boostContainer;
        private readonly Dictionary<string, BoostSettingsData> _concreteBoostActivatorsSettings;

        private float _initialBallSpeed;
        private readonly Dictionary<Type,IConcreteBoostActivator> _concreteBoostActivators;

        public BoostsActivator(
            IBoostContainer boostContainer,
            Dictionary<string, BoostSettingsData> concreteBoostActivatorsSettingsSettings,
            List<IConcreteBoostActivator> concreteBoostActivators)
        {
            _boostContainer = boostContainer;
            _concreteBoostActivatorsSettings = concreteBoostActivatorsSettingsSettings;
            _concreteBoostActivators = concreteBoostActivators
                .ToDictionary(x => x.GetType(), x => x);

            _boostContainer.BoostEnded += OnBoostEnded;
        }

        public void Activate(BoostView view)
        {
            string boostId = view.BoostTypeId;

            bool activated = ActivateBoostById(view.BoostTypeId);

            if (activated && _concreteBoostActivatorsSettings[boostId].ConcreteBoostActivator.IsTimeableBoost)
            {
                _boostContainer.AddBoost(boostId);
            }
        }

        private void OnBoostEnded(string boostId)
        {
            DeactivateBoostById(boostId);
        }

        public void LoadProgress(LevelDataProgress levelDataProgress)
        {
            foreach (SaveActiveBoostData boostData in levelDataProgress.ActiveBoostDatas)
            {
                ActivateBoostById(boostData.BoostTypeId);
            }
        }

        private bool ActivateBoostById(string id)
        {
            BoostSettingsData boostSettingsData = _concreteBoostActivatorsSettings[id];
            Type activatorType = boostSettingsData.ConcreteBoostActivator.GetType();

            if (_concreteBoostActivators.ContainsKey(activatorType))
            {
                _concreteBoostActivators[activatorType].Activate(boostSettingsData.BoostDataProvider);
                return true;
            }

            return false;
        }
        
        private bool DeactivateBoostById(string id)
        {
            BoostSettingsData boostSettingsData = _concreteBoostActivatorsSettings[id];
            Type activatorType = boostSettingsData.ConcreteBoostActivator.GetType();

            if (_concreteBoostActivators.ContainsKey(activatorType))
            {
                _concreteBoostActivators[activatorType].Deactivate();
                return true;
            }

            return false;
        }
    }
}