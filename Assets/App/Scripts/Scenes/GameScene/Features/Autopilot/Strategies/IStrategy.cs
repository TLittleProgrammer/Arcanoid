namespace App.Scripts.Scenes.GameScene.Features.Autopilot
{
    public interface IStrategy
    {
        NodeStatus Process();
        void Reset();
    }
}