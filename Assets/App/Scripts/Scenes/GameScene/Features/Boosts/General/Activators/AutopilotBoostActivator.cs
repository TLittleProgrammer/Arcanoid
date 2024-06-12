using App.Scripts.Scenes.GameScene.Features.Boosts.Autopilot;
using App.Scripts.Scenes.GameScene.Features.Boosts.General.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Components;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape.Move;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators
{
    public class AutopilotBoostActivator : IConcreteBoostActivator, ITickable, IActivable
    {
        private BehaviourTree _behaviourTree;
        private IPlayerShapeMover _playerShapeMover;
        
        public bool IsActive { get; set; }
        
        public AutopilotBoostActivator(BehaviourTree behaviourTree, IPlayerShapeMover playerShapeMover)
        {
            _behaviourTree = behaviourTree;
            _playerShapeMover = playerShapeMover;
        }

        public bool IsTimeableBoost => true;

        public void Activate()
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

        public void Deactivate()
        {
            _playerShapeMover.IsActive = true;
            IsActive = false;
        }
    }
}