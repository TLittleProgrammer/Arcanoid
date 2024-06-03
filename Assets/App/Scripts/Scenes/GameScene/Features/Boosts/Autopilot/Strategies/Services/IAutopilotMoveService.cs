using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.Autopilot.Strategies
{
    public interface IAutopilotMoveService
    {
        void Move(Vector3 targetPosition);
    }
}