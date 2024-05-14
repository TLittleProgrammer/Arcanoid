using System.Collections.Generic;
using DG.Tweening;

namespace App.Scripts.Scenes.GameScene.Dotween
{
    public class TweenersLocator : ITweenersLocator
    {
        private List<Tweener> _tweeners = new();
        
        public void AddTweener(Tweener tweener)
        {
            _tweeners.Add(tweener);
        }

        public void RemoveTweener(Tweener tweener)
        {
            tweener.Kill();
            _tweeners.Remove(tweener);
        }

        public void RemoveAllTweeners()
        {
            foreach (Tweener tweener in _tweeners)
            {
                tweener.Kill();
            }
            
            _tweeners.Clear();
        }
    }
}