using App.Scripts.Scenes.GameScene.Features.Autopilot.Nodes;

namespace App.Scripts.Scenes.GameScene.Features.Autopilot.Strategies
{
    public interface IStrategy
    {
        NodeStatus Process();
        void Reset();
    }
}