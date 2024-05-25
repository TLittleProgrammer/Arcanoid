using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.GameScene.Features.LevelView
{
    public sealed class LevelPackBackgroundView : MonoBehaviour, ILevelPackBackgroundView
    {
        [SerializeField] private Image _background;

        public Image Background => _background;
    }
}