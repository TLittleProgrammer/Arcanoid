using App.Scripts.Scenes.GameScene.Features.Boosts.General.Systems;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape.Move;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators
{
    public class AutopilotBoostActivator : IConcreteBoostActivator
    {
        private IAutopilotSystem _autopilotSystem;
        private IPlayerShapeMover _playerShapeMover;
        
        public AutopilotBoostActivator(IAutopilotSystem autopilotSystem, IPlayerShapeMover playerShapeMover)
        {
            _autopilotSystem = autopilotSystem;
            _playerShapeMover = playerShapeMover;
        }

        public bool IsTimeableBoost => true;

        public void Activate()
        {
            SetFlag(true);
            
        }

        public void Deactivate()
        {
            SetFlag(false);
        }

        private void SetFlag(bool flag)
        {
            _autopilotSystem.IsActive = flag;
            _playerShapeMover.IsActive = !flag;
        }
    }
}