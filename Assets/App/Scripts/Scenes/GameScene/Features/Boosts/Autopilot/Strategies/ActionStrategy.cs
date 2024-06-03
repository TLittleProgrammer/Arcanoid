using System;
using App.Scripts.Scenes.GameScene.Features.Boosts.Autopilot.Nodes;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.Autopilot.Strategies
{
    public class ActionStrategy : IStrategy
    {
        private readonly Action _doSomething;

        public ActionStrategy(Action doSomething)
        {
            _doSomething = doSomething;
        }
        
        public NodeStatus Process()
        {
            _doSomething();
            return NodeStatus.Success;
        }
    }
}