using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators;
using App.Scripts.Scenes.GameScene.Features.Boosts.General.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress.Data;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General
{
    public class BoostsActivator : IBoostsActivator, IInitializeByLevelProgress
    {
        private readonly IBoostContainer _boostContainer;
        private readonly Dictionary<string, BoostSettingsData> _concreteBoostActivatorsSettings;
        private readonly DiContainer _diContainer;

        private float _initialBallSpeed;
        private readonly Dictionary<Type,IConcreteBoostActivator> _concreteBoostActivators = new();
        

        public BoostsActivator(
            IBoostContainer boostContainer,
            Dictionary<string, BoostSettingsData> concreteBoostActivatorsSettingsSettings,
            DiContainer diContainer)
        {
            _boostContainer = boostContainer;
            _concreteBoostActivatorsSettings = concreteBoostActivatorsSettingsSettings;
            _diContainer = diContainer;

            _boostContainer.BoostEnded += OnBoostEnded;
            _boostContainer.DeactivateBoost += DeactivateBoostById;
        }

        public void Activate(BoostView view)
        {
            string boostId = view.BoostTypeId;
            
            if (_concreteBoostActivatorsSettings[boostId].ConcreteBoostActivator.IsTimeableBoost)
            {
                _boostContainer.AddBoost(boostId);
            }
            
            ActivateBoostById(view.BoostTypeId);
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

        private void ActivateBoostById(string id)
        {
            BoostSettingsData boostSettingsData = _concreteBoostActivatorsSettings[id];
            Type activatorType = boostSettingsData.ConcreteBoostActivator.GetType();

            if (!_concreteBoostActivators.ContainsKey(activatorType))
            {
                var activator = InitializeActivator(boostSettingsData, activatorType);
                activator.Activate();

                if (boostSettingsData.ConcreteBoostActivator.IsTimeableBoost)
                {
                    _concreteBoostActivators.Add(activatorType, activator);
                }
            }
        }

        private IConcreteBoostActivator InitializeActivator(BoostSettingsData boostSettingsData, Type activatorType)
        {
            IConcreteBoostActivator activator;

            if (boostSettingsData.BoostDataProvider is not null)
            {
                activator = (IConcreteBoostActivator)_diContainer.Instantiate(activatorType,
                    new[] { boostSettingsData.BoostDataProvider });
            }
            else
            {
                activator = (IConcreteBoostActivator)_diContainer.Instantiate(activatorType);
            }

            return activator;
        }

        private void DeactivateBoostById(string id)
        {
            BoostSettingsData boostSettingsData = _concreteBoostActivatorsSettings[id];
            Type activatorType = boostSettingsData.ConcreteBoostActivator.GetType();

            if (_concreteBoostActivators.ContainsKey(activatorType))
            {
                _concreteBoostActivators[activatorType].Deactivate();
                _concreteBoostActivators.Remove(activatorType);
            }
        }
    }
}