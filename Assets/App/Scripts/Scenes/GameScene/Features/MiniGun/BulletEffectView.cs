using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.MiniGun
{
    public class BulletEffectView : MonoBehaviour
    {
        public ParticleSystem ParticleSystem;

        public class Pool : MonoMemoryPool<BulletEffectView>
        {
            
        }
    }
}