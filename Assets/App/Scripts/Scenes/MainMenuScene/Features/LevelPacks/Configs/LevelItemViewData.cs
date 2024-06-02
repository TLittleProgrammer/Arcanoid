using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenuScene.Features.LevelPacks.Configs
{
    public class LevelItemViewData
    {
        [BoxGroup("TopRight")]
        public Sprite GalacticPassedLevelsBackground;
        [BoxGroup("TopRight")]
        public Color PassedLevelsColor;
        [BoxGroup("General")]
        public Sprite BigBackground;
        [BoxGroup("General")]
        public Sprite Glow;
        [BoxGroup("General")]
        public Color InactiveColorForNaming;
        [BoxGroup("Left")]
        public Sprite LeftImageHalf;
        [BoxGroup("Left")]
        public Sprite MaskableImage;
    }
}