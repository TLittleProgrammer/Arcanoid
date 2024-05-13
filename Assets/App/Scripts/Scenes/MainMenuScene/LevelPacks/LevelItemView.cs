﻿using System;
using App.Scripts.External.Localisation.MonoBehaviours;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenuScene.LevelPacks
{
    public class LevelItemView : MonoBehaviour, ILevelItemView
    {
        [BoxGroup("TopRight"), SerializeField]
        private TMP_Text _galacticPassedLevels;
        [BoxGroup("TopRight"), SerializeField]
        private Image _galacticPassedLevelsBackground;
        [BoxGroup("General"), SerializeField]
        private Image _bigBackground;
        [BoxGroup("General"), SerializeField]
        private UILocale _galacticName;
        [BoxGroup("General"), SerializeField]
        private Image _glow;
        [BoxGroup("Left"), SerializeField]
        private Image _leftImageHalf;
        [BoxGroup("Left"), SerializeField]
        private Image _galacticIcon;
        [BoxGroup("Left"), SerializeField]
        private Image _lockIcon;

        [BoxGroup("Left"), SerializeField]
        private Image _maskableImage;

        public event Action Clicked;

        public TMP_Text GalacticPassedLevels => _galacticPassedLevels;
        public Image GalacticPassedLevelsBackground => _galacticPassedLevelsBackground;
        public Image BigBackground => _bigBackground;
        public UILocale GalacticName => _galacticName;
        public Image Glow => _glow;
        public Image LeftImageHalf => _leftImageHalf;
        public Image GalacticIcon => _galacticIcon;
        public Image LockIcon => _lockIcon;
        public Image MaskableImage => _maskableImage;
        public GameObject GameObject => gameObject;

        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked?.Invoke();
        }
    }
}