using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Effects
{
    public interface IEffect<TEffect> where TEffect : MonoBehaviour
    {
        void PlayEffect();
        void StopEffect();
        
        public class Pool : MonoMemoryPool<TEffect>
        {
            
        }
    }
}