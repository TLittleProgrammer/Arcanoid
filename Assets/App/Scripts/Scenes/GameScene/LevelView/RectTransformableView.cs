using App.Scripts.General.Components;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.LevelView
{
    public class RectTransformableView : MonoBehaviour, IRectTransformable
    {
        [SerializeField] private RectTransform _rectTransform;
        
        public RectTransform RectTransform => _rectTransform;
    }
}