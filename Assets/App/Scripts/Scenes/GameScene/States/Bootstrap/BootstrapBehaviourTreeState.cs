using App.Scripts.External.GameStateMachine;
using App.Scripts.Scenes.GameScene.Features.Boosts.Autopilot;
using App.Scripts.Scenes.GameScene.Features.Boosts.Autopilot.Nodes;
using App.Scripts.Scenes.GameScene.Features.Boosts.Autopilot.Strategies;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.States.Bootstrap
{
    public class BootstrapBehaviourTreeState : IState
    {
        private readonly IStateMachine _stateMachine;
        private readonly BehaviourTree _behaviourTree;
        private readonly SimpleMovingStrategy _simpleMovingStrategy;

        public BootstrapBehaviourTreeState(
            IStateMachine stateMachine,
            BehaviourTree behaviourTree,
            SimpleMovingStrategy simpleMovingStrategy)
        {
            _stateMachine = stateMachine;
            _behaviourTree = behaviourTree;
            _simpleMovingStrategy = simpleMovingStrategy;
        }
        
        public async UniTask Enter()
        {
            _behaviourTree.AddChild(CreateNode("SimpleMoving", _simpleMovingStrategy));

            _stateMachine.Enter<BootstrapItemsDestroyerState>();
            
            await UniTask.CompletedTask;
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
        
        private Node CreateNode(string nodeName, IStrategy strategy)
        {
            return new SimpleMovingNode(nodeName, strategy);
        }
    }
}