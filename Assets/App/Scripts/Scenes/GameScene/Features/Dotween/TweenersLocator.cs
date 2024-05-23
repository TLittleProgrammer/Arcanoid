using System.Collections.Generic;
using DG.Tweening;

namespace App.Scripts.Scenes.GameScene.Dotween
{
    public class TweenersLocator : ITweenersLocator
    {
        private List<Tweener> _tweeners = new();
        private List<Sequence> _sequences = new();
        
        public void AddTweener(Tweener tweener)
        {
            _tweeners.Add(tweener);
        }

        public void AddSequence(Sequence sequence)
        {
            _sequences.Add(sequence);
        }

        public void RemoveTweener(Tweener tweener)
        {
            tweener.Kill();
            _tweeners.Remove(tweener);
        }

        public void RemoveAll()
        {
            foreach (Tweener tweener in _tweeners.ToArray())
            {
                tweener.Kill();
            }

            foreach (Sequence sequence in _sequences.ToArray())
            {
                sequence.Kill();
            }
            
            _sequences.Clear();
            _tweeners.Clear();
        }
    }
}