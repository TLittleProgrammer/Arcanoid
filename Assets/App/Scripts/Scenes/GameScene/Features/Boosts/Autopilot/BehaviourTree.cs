using App.Scripts.Scenes.GameScene.Features.Boosts.Autopilot.Nodes;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.Autopilot
{
    public class BehaviourTree : Node
    {
        
        public BehaviourTree()
        {
        
        }
        
        public BehaviourTree(string name) : base(name)
        {
            
        }

        public override NodeStatus Process()
        {
            while (CurrentChild < Children.Count)
            {
                NodeStatus status = Children[CurrentChild].Process();

                if (status != NodeStatus.Success)
                {
                    return status;
                }

                CurrentChild++;
            }

            CurrentChild = 0;
            return NodeStatus.Success;
        }
    }
}