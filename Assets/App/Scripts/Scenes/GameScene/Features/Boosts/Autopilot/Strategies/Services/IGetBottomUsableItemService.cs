using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.Autopilot.Strategies
{
    public interface IGetBottomUsableItemService
    {
        Vector3 GetBottomPosition();
    }
}