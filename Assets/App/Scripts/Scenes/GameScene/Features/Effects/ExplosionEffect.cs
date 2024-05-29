using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Effects
{
    public class ExplosionEffect : MonoBehaviour
    {
        public ParticleSystem Explosion;

        public class Pool : MonoMemoryPool<ExplosionEffect>
        {
            
        }
    }
}