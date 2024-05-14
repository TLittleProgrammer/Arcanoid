﻿using App.Scripts.Scenes.GameScene.Dotween;
using App.Scripts.Scenes.GameScene.Settings;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Popups.Win
{
    public class CircleWinEffect : MonoBehaviour
    {
        [SerializeField] private Transform _effect;
        
        [Inject]
        private void Construct(CircleWinEffectSettings settings, ITweenersLocator tweenersLocator)
        {
            tweenersLocator.AddTweener(_effect
                .DORotate(new Vector3(0f, 0f, -360f), settings.Duration, RotateMode.LocalAxisAdd)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear));
        }
    }
}