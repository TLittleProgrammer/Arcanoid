using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Blocks
{
    public interface IBlockShakeService
    {
        void Shake(Transform transform);
    }
}