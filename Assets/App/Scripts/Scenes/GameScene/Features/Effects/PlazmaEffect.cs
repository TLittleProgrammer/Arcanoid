using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Effects
{
    public class PlazmaEffect : MonoEffect
    {
        public ParticleSystem ParticleSystem;
        
        public override void PlayEffect(Transform initialEntityTransform, Transform colliderEntityTransform)
        {
            ParticleSystem.MainModule mainModule = ParticleSystem.main;
            
            if (colliderEntityTransform.position.x > 1f)
            {
                mainModule.startRotation = 180f * Mathf.Deg2Rad;
            }
            else if(colliderEntityTransform.position.x < -1f)
            {
                mainModule.startRotation = 0f;
            }
            else
            {
                mainModule.startRotation = 90f * Mathf.Deg2Rad;
            }

            transform.position = initialEntityTransform.position;
            ParticleSystem.Play();
        }
    }
}