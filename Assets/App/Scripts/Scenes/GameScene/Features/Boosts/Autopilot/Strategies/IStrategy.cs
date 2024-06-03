using App.Scripts.Scenes.GameScene.Features.Boosts.Autopilot.Nodes;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.Autopilot.Strategies
{
    public interface IStrategy
    {
        NodeStatus Process();

        void Reset()
        {
            
        }
    }
}