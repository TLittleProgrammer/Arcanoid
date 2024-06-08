using System;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Effects
{
    public abstract class MonoEffect : MonoBehaviour, IEffect
    {
        public event Action<IEffect> Disabled;
        public abstract void PlayEffect(Transform initialEntityTransform, Transform colliderEntityTransform);

        private void OnDisable()
        {
            Disabled?.Invoke(this);
        }
    }
}