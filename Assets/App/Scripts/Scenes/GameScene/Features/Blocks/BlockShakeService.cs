using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Entities;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Blocks
{
    public sealed class BlockShakeService : IBlockShakeService
    {
        private Dictionary<IEntityView, Sequence> _sequences = new();

        public void Shake(IEntityView entityView)
        {
            if (_sequences.ContainsKey(entityView))
            {
                _sequences[entityView].Restart();
                return;
            }

            Sequence sequence = DOTween.Sequence();

            Vector2 initialPoint = entityView.Position;
            
            sequence
                .Append(entityView.GameObject.transform.DOShakePosition(0.5f, 0.15f))
                .OnComplete(() =>
                {
                    entityView.Position = initialPoint;
                    sequence.Kill();
                    _sequences.Remove(entityView);
                });
            
            _sequences.Add(entityView, sequence);
        }
    }
}