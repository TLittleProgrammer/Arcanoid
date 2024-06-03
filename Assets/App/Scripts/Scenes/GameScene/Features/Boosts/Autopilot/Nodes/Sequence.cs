using App.Scripts.Scenes.GameScene.Features.Restart;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.Autopilot.Nodes
{
    public class Sequence : Node
    {
        public Sequence(string name) : base(name) {}

        public override NodeStatus Process()
        {
            if (CurrentChild < Children.Count)
            {
                switch (Children[CurrentChild].Process()) 
                {
                    case NodeStatus.Running:
                        return NodeStatus.Running;
                    case NodeStatus.Failure:
                        Reset();
                        return NodeStatus.Failure;
                    default:
                        CurrentChild++;
                        return CurrentChild == Children.Count ? NodeStatus.Success : NodeStatus.Running;
                }
            }
            
            Reset();
            return NodeStatus.Success;
        }
    }
}