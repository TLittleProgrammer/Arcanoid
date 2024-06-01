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
        private readonly BoostViewProvider _boostViewProvider;

        public BoostItemViewFactory(BoostItemView prefab, BoostViewProvider boostViewProvider)
        {
            _prefab = prefab;
            _boostViewProvider = boostViewProvider;
        }
        
        public BoostItemView Create(BoostTypeId param)
        {
            BoostItemView spawned = Object.Instantiate(_prefab);

            spawned.BoostIcon.sprite = _boostViewProvider.Sprites[param];
            
            return spawned;
        }
    }
}