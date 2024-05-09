using App.Scripts.Scenes.GameScene.Interfaces;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Ball
{
    public class BallView : MonoBehaviour, ITransformable
    {
        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }
    }
}