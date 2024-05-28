using App.Scripts.Scenes.GameScene.Features.Autopilot.Strategies;

namespace App.Scripts.Scenes.GameScene.Features.Autopilot.Nodes
{
    public class SimpleMovingNode : Node
    {
        private readonly IStrategy _strategy;

        public SimpleMovingNode(string name, IStrategy strategy) : base(name)
        {
            _strategy = strategy;
        }

        public override NodeStatus Process()
        {
            return _strategy.Process();
        }

        public override void Reset()
        {
            _strategy.Reset();
        }
    }
}