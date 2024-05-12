using System.Collections.Generic;
using DG.Tweening;

namespace App.Scripts.General.DotweenContainerService
{
    public class DotweenContainerService : IDotweenContainerService
    {
        private List<Tween> _tweens = new();

        public void AddTween(Tween tween)
        {
            _tweens.Add(tween);
            tween.onStepComplete += () =>
            {
                RemoveTween(tween);
            };
        }

        public void RemoveTween(Tween tween)
        {
            if (_tweens.Contains(tween))
            {
                _tweens.Remove(tween);
            }
        }
    }
}