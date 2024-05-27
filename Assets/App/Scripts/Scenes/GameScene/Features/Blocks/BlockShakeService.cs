using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Blocks
{
    public sealed class BlockShakeService : IBlockShakeService
    {
        private Dictionary<Transform, Sequence> _sequences = new();

        public void Shake(Transform transform)
        {
            if (_sequences.ContainsKey(transform))
            {
                _sequences[transform].Restart();
                return;
            }

            Sequence sequence = DOTween.Sequence();

            Vector3 initialPoint = transform.position;
            
            sequence
                .Append(transform.DOShakePosition(0.5f, 0.15f))
                .OnComplete(() =>
                {
                    transform.position = initialPoint;
                    sequence.Kill();
                    _sequences.Remove(transform);
                });
            
            _sequences.Add(transform, sequence);
        }
    }
}