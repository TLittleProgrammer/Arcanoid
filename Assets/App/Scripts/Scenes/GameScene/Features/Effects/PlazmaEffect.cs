using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Effects
{
    public class PlazmaEffect : MonoBehaviour
    {
        public ParticleSystem ParticleSystem;

        public class Pool : MonoMemoryPool<PlazmaEffect>
        {
            
        }
    }
}