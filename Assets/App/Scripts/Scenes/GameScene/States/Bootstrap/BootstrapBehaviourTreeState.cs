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
        private readonly StickyStrategy _stickyStrategy;

        public BootstrapBehaviourTreeState(
            IStateMachine stateMachine,
            BehaviourTree behaviourTree,
            SimpleMovingStrategy simpleMovingStrategy,
            StickyStrategy stickyStrategy)
        {
            _stateMachine = stateMachine;
            _behaviourTree = behaviourTree;
            _simpleMovingStrategy = simpleMovingStrategy;
            _stickyStrategy = stickyStrategy;
        }
        
        public UniTask Enter()
        {
            Sequence sequence = new Sequence("Moving");
            
            //sequence.AddChild(CreateNode("StickyMoving", _stickyStrategy));
            sequence.AddChild(CreateNode("SimpleMoving", _simpleMovingStrategy));
            
            _behaviourTree.AddChild(sequence);

            _stateMachine.Enter<BootstrapEntityDestroyerState>().Forget();
            
            return UniTask.CompletedTask;
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