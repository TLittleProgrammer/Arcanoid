using System;
using App.Scripts.Scenes.GameScene.Features.Boosts.Autopilot.Nodes;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.Autopilot.Strategies
{
    public class Condition : IStrategy
    {
        private readonly Func<bool> _predicate;

        public Condition(Func<bool> predicate)
        {
            _predicate = predicate;
        }
        
        public NodeStatus Process()
        {
            return _predicate() ? NodeStatus.Success : NodeStatus.Failure;
        }
    }
}