using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Healthes.View
{
    public sealed class HealthPointView : MonoBehaviour, IHealthPointView
    {
        [SerializeField] private Image _image;

        public Image Image => _image;

        public class Pool : MonoMemoryPool<HealthPointView>
        {
            
        }
    }
}