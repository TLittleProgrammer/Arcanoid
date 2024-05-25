using App.Scripts.Scenes.GameScene.Features.Entities;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.UI
{
    public class BoostItemView : MonoBehaviour
    {
        public Image BoostIcon;
        public Image ScollImage;

        public class Factory : PlaceholderFactory<BoostTypeId, BoostItemView>
        {
            
        }
    }
}