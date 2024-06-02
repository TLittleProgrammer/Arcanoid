using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Bullets
{
    public class BulletEffectView : MonoBehaviour
    {
        public ParticleSystem ParticleSystem;

        public class Pool : MonoMemoryPool<BulletEffectView>
        {
            
        }
    }
}