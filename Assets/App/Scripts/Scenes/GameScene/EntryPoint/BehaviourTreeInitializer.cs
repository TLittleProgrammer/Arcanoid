using App.Scripts.Scenes.GameScene.Features.Autopilot;
using App.Scripts.Scenes.GameScene.Features.Autopilot.Nodes;
using App.Scripts.Scenes.GameScene.Features.Autopilot.Strategies;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint
{
    public class BehaviourTreeInitializer : IInitializable
    {
        private readonly BehaviourTree _behaviourTree;
        private readonly SimpleMovingStrategy _simpleMovingStrategy;

        public BehaviourTreeInitializer(
            BehaviourTree behaviourTree,
            SimpleMovingStrategy simpleMovingStrategy)
        {
            _behaviourTree = behaviourTree;
            _simpleMovingStrategy = simpleMovingStrategy;
        }
        
        public void Initialize()
        {
            _behaviourTree.AddChild(CreateNode("SimpleMoving", _simpleMovingStrategy));
        }

        private Node CreateNode(string nodeName, IStrategy strategy)
        {
            return new SimpleMovingNode(nodeName, strategy);
        }
    }
}