using App.Scripts.External.Localisation.MonoBehaviours;
using App.Scripts.General.Components;
using App.Scripts.Scenes.GameScene.Components;
using TMPro;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenuScene.LevelPacks
{
    public interface ILevelItemView : IGameObjectable, IClickable
    {
        TMP_Text GalacticPassedLevels { get; }
        Image GalacticPassedLevelsBackground { get; }
        Image BigBackground { get; }
        UILocale GalacticName { get; }
        Image Glow { get; }
        Image LeftImageHalf { get; }
        Image GalacticIcon { get; }
        Image LockIcon { get; }
        Image MaskableImage { get; }
    }
}