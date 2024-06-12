using App.Scripts.General.Providers;
using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using App.Scripts.Scenes.GameScene.Features.Boosts.General.UI;
using App.Scripts.Scenes.GameScene.Features.Settings;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Factories.Boosts
{
    public class BoostItemViewFactory : IFactory<BoostTypeId, BoostItemView>
    {
        private readonly BoostItemView _prefab;
        private readonly SpriteProvider _spriteProvider;

        public BoostItemViewFactory(BoostItemView prefab, SpriteProvider spriteProvider)
        {
            _prefab = prefab;
            _spriteProvider = spriteProvider;
        }
        
        public BoostItemView Create(BoostTypeId boostId)
        {
            BoostItemView spawned = Object.Instantiate(_prefab);

            spawned.BoostIcon.sprite = _spriteProvider.Sprites[boostId.ToString() + "_block"];
            
            return spawned;
        }
    }
}