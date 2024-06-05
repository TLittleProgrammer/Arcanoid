using System;
using App.Scripts.Scenes.GameScene.Features.Components;
using App.Scripts.Scenes.GameScene.Features.Effects.ObjectPool;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Effects
{
    [RequireComponent(typeof(ParticleSystem))]
    public abstract class ParticleSystemEffect : AbstractEffect
    {
        private ParticleSystem.MainModule _particlesMainModule;
        
        protected void Awake()
        {
            _particlesMainModule = ParticleSystem.main;
        }
        
        public override float Scale
        {
            get => _particlesMainModule.startSize.constant;
            set => _particlesMainModule.startSize = value;
        }

        public override void PlayEffect()
        {
            ParticleSystem.Play();
        }

        public override void StopEffect()
        {
            ParticleSystem.Stop();
        }
    }
}