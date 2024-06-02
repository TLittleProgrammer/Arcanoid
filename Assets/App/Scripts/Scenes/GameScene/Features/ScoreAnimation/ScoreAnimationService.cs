using System;
using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Settings;
using DG.Tweening;
using TMPro;

namespace App.Scripts.Scenes.GameScene.Features.ScoreAnimation
{
    public sealed class ScoreAnimationService : IScoreAnimationService
    {
        private readonly ScoreAnimationSettings _scoreAnimationSettings;
        
        private Dictionary<TMP_Text, Tweener> _sequences = new();

        public ScoreAnimationService(ScoreAnimationSettings scoreAnimationSettings)
        {
            _scoreAnimationSettings = scoreAnimationSettings;
        }

        public void Animate(TMP_Text text, int from, int to, Action<int> updateValue)
        {
            if (!_sequences.ContainsKey(text))
            {
                _sequences.Add(
                    text,
                    DOVirtual.Int(from, to, _scoreAnimationSettings.Duration, updateValue.Invoke)
                );
                
                return;
            }
            
            _sequences[text].Kill();
            _sequences[text] = DOVirtual.Int(from, to, _scoreAnimationSettings.Duration, updateValue.Invoke);
        }
    }
}