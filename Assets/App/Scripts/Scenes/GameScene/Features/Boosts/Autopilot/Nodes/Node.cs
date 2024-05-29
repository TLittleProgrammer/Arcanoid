using System.Collections.Generic;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.Autopilot.Nodes
{
    public class Node
    {
        public readonly string Name;
        public readonly List<Node> Children = new();
        
        public NodeStatus Status;

        protected int CurrentChild;
        
        public Node(string name = "Node")
        {
            Name = name;
        }

        public void AddChild(Node child)
        {
            Children.Add(child);
        }

        public virtual NodeStatus Process()
        {
            return Children[CurrentChild].Process();
        }

        public virtual void Reset()
        {
            CurrentChild = 0;

            foreach (Node child in Children)
            {
                child.Reset();
            }
        }
    }
}