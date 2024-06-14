using App.Scripts.Scenes.GameScene.Features.Boosts.Autopilot;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Systems
{
    public class AutopilotSystem : IAutopilotSystem
    {
        private readonly BehaviourTree _behaviourTree;

        public AutopilotSystem(BehaviourTree behaviourTree)
        {
            _behaviourTree = behaviourTree;
        }
        
        public bool IsActive { get; set; }

        public void Tick()
        {
            if (IsActive is false)
                return;

            _behaviourTree.Process();
        }
    }
}