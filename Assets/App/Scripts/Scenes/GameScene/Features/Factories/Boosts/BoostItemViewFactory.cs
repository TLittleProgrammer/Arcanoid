using App.Scripts.General.Providers;
using App.Scripts.Scenes.GameScene.Features.Boosts.General.UI;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Factories.Boosts
{
    public class BoostItemViewFactory : IFactory<string, BoostItemView>
    {
        private readonly BoostItemView _prefab;
        private readonly SpriteProvider _spriteProvider;

        public BoostItemViewFactory(BoostItemView prefab, SpriteProvider spriteProvider)
        {
            _prefab = prefab;
            _spriteProvider = spriteProvider;
        }
        
        public BoostItemView Create(string boostId)
        {
            BoostItemView spawned = Object.Instantiate(_prefab);

            spawned.BoostIcon.sprite = _spriteProvider.Sprites[boostId + "_block"];
            
            return spawned;
        }
    }
}