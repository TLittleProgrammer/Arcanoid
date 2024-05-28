using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Effects.Bombs
{
    public class LaserEffect : MonoBehaviour
    {
        public ParticleSystem Laser;

        public class Pool : MonoMemoryPool<LaserEffect>
        {
            
        }
    }
}