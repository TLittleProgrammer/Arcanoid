using App.Scripts.Scenes.GameScene.Entities;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Effects
{
    public class CircleEffect : ParticleSystemEffect<CircleEffect>
    {
        public class Factory : PlaceholderFactory<EntityView, CircleEffect>
        {
            
        }
    }
}