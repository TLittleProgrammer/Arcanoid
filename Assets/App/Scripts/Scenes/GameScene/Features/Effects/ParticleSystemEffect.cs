﻿using App.Scripts.Scenes.GameScene.Features.Effects.ObjectPool;
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
        
        public float Scale
        {
            set => _particlesMainModule.startSize = value;
        }
    }
}