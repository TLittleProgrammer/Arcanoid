using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.General.AnimatableButtons.Settings
{
    [CreateAssetMenu(menuName = "Configs/Settings/Dotweens/Dotween Animatable Button", fileName = "DotweenANimatableButtonSettings")]
    public class DotweenAnimatableButtonSettings : SerializedScriptableObject
    {
        public Vector3 TargetScale;
        public float Duration;
        public Ease Ease;
    }
}