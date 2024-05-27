using App.Scripts.Scenes.GameScene.Features.Autopilot;
using App.Scripts.Scenes.GameScene.Features.Boosts.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Components;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.PlayerShape.Move;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.Helpers
{
    public class AutopilotBoostActivator : IConcreteBoostActivator, ITickable, IActivable
    {
        private readonly BehaviourTree _behaviourTree;
        private readonly IPlayerShapeMover _playerShapeMover;
        public bool IsActive { get; set; }
        
        public AutopilotBoostActivator(
            BehaviourTree behaviourTree,
            IBoostContainer boostContainer,
            IPlayerShapeMover playerShapeMover)
        {
            _behaviourTree = behaviourTree;
            _playerShapeMover = playerShapeMover;

            boostContainer.BoostEnded += OnBoostEnded;
        }

        public void Activate(BoostTypeId boostTypeId)
        {
            IsActive = true;
            _playerShapeMover.IsActive = false;
        }

        public void Tick()
        {
            if (IsActive is false)
            {
                return;
            }

            _behaviourTree.Process();
        }

        private void OnBoostEnded(BoostTypeId boostType)
        {
            if (boostType is BoostTypeId.Autopilot)
            {
                _playerShapeMover.IsActive = true;
                IsActive = false;
            }
        }
    }
}