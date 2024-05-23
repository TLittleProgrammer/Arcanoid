using App.Scripts.External.Components;
using UnityEngine.UI;

namespace App.Scripts.Scenes.GameScene.Features.Components
{
    public interface IImagable : IComponent
    {
        public Image Image { get; }
    }
}