using UnityEngine;

namespace App.Scripts.Scenes.GameScene.PlayerShape
{
    public sealed class PlayerView : MonoBehaviour, ITransformable
    {
        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }
    }
}