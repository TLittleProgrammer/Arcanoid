using System;
using App.Scripts.External.Localisation.MonoBehaviours;
using App.Scripts.General.Levels;
using App.Scripts.Scenes.MainMenuScene.LevelPacks.Configs;
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
        private TMP_Text _galacticText;
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
        public TMP_Text GalacticText => _galacticText;
        public UILocale GalacticName => _galacticName;
        public Image Glow => _glow;
        public Image LeftImageHalf => _leftImageHalf;
        public Image GalacticIcon => _galacticIcon;
        public Image LockIcon => _lockIcon;
        public Image MaskableImage => _maskableImage;

        public GameObject GameObject => gameObject;

        public void UpdateVisual(LevelModel levelModel)
        {
            UpdateGeneralViews(levelModel.LevelViewData, levelModel.LevelPack);
            UpdateByItemTypeId(levelModel.LevelViewData, levelModel.LevelPack,levelModel.VisualTypeId, levelModel.PassedLevels);
        }

        private void UpdateByItemTypeId(LevelItemViewData levelViewData, LevelPack levelPack, VisualTypeId visualType, int passedLevels)
        {
            if (visualType is VisualTypeId.NotOpened)
            {
                GalacticPassedLevels.text = $"0/{levelPack.Levels.Count}";
                GalacticIcon.gameObject.SetActive(false);
                LockIcon.gameObject.SetActive(true);

                GalacticName.Text.colorGradient = new VertexGradient(Color.white, Color.white, Color.white, Color.white);
                GalacticText.colorGradient      = new VertexGradient(Color.white, Color.white, Color.white, Color.white);
                GalacticName.Text.color         = levelViewData.InactiveColorForNaming;
                GalacticText.color              = levelViewData.InactiveColorForNaming;
                GalacticPassedLevels.color      = levelViewData.PassedLevelsColor;
            }
            else
            {
                GalacticIcon.sprite = levelPack.GalacticIcon;
                GalacticPassedLevels.text = $"{passedLevels}/{levelPack.Levels.Count}";
                GalacticIcon.gameObject.SetActive(true);
                LockIcon.gameObject.SetActive(false);
            }
        }

        private void UpdateGeneralViews(LevelItemViewData levelViewData, LevelPack levelPack)
        {
            Glow.sprite = levelViewData.Glow;
            BigBackground.sprite = levelViewData.BigBackground;
            MaskableImage.sprite = levelViewData.MaskableImage;
            LeftImageHalf.sprite = levelViewData.LeftImageHalf;
            GalacticPassedLevelsBackground.sprite = levelViewData.GalacticPassedLevelsBackground;
            GalacticName.SetToken(levelPack.LocaleKey);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked?.Invoke();
        }
    }
}