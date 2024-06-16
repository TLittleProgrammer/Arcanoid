using App.Scripts.Scenes.GameScene.Features.Effects;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Bullets
{
    public class BulletEffectView : MonoEffect
    {
        public ParticleSystem ParticleSystem;
        
        public override void PlayEffect(Transform initialEntityTransform, Transform colliderEntityTransform)
        {
            transform.position = initialEntityTransform.transform.position + Vector3.down * 0.25f;
            ParticleSystem.Play();
        }
    }
}