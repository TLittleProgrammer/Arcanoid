using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Shake
{
    public interface IShakeService
    {
        void Shake(Transform transform);
    }
}