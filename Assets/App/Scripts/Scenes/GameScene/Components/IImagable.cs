using App.Scripts.External.Components;
using UnityEngine.UI;

namespace App.Scripts.Scenes.GameScene.Components
{
    public interface IImagable : IComponent
    {
        public Image Image { get; }
    }
}