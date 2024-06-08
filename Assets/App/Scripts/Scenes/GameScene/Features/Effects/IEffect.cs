using System;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Effects
{
    public interface IEffect
    {
        event Action<IEffect> Disabled;
        void PlayEffect(Transform initialEntityTransform, Transform colliderEntityTransform);
    }
}