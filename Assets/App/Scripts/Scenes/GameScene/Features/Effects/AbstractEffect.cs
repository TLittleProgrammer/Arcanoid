using App.Scripts.Scenes.GameScene.Features.Components;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Effects
{
    public class AbstractEffect : MonoEffect, IPositionable, IScalable<float>
    {
        [SerializeField] protected ParticleSystem ParticleSystem;

        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public float Scale { get; set; }
        public GameObject GameObject => gameObject;

        public override void PlayEffect(Transform initialEntityTransform, Transform colliderEntityTransform) { }
    }
}