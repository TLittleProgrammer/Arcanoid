using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Effects
{
    public class ExplosionEffect : MonoEffect
    {
        public ParticleSystem Explosion;

        public override void PlayEffect(Transform initialEntityTransform, Transform colliderEntityTransform)
        {
            ParticleSystem.MainModule explosionMain = Explosion.main;
            explosionMain.startSizeX = initialEntityTransform.transform.localScale.x * 1.431f;
            transform.position = initialEntityTransform.position;
            Explosion.Play();
        }
    }
}