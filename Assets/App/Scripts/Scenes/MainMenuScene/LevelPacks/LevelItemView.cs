using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenuScene.LevelPacks
{
    public class LevelItemView : MonoBehaviour
    {
        [BoxGroup("TopRight")]
        public TMP_Text GalacticPassedLevels;
        [BoxGroup("TopRight")]
        public Image GalacticPassedLevelsBackground;
        [BoxGroup("General")]
        public Image BigBackground;
        [BoxGroup("General")]
        public TMP_Text _galacticName;
        [BoxGroup("General")]
        public Image Glow;
        [BoxGroup("Left")]
        public Image LeftImageHalf;
        [BoxGroup("Left")]
        public Image GalacticIcon;
        [BoxGroup("Left")]
        public Image MaskableImage;
    }
}