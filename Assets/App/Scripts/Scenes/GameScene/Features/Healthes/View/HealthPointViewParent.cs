using App.Scripts.External.Components;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Healthes.View
{
    public class HealthPointViewParent : MonoBehaviour, ITransformable
    {
        public Transform Transform => transform;
    }
}