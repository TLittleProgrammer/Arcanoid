using App.Scripts.External.Components;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenuScene.Features.LevelPacks.MonoBehaviours
{
    public class LevelPackParent : MonoBehaviour, ITransformable
    {
        public Transform Transform => transform;
    }
}