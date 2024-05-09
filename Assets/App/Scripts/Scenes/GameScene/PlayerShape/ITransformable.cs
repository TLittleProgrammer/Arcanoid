using UnityEngine;

namespace App.Scripts.Scenes.GameScene.PlayerShape
{
    public interface ITransformable
    {
        Vector3 Position { get; set; }
    }
}