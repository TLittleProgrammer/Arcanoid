using App.Scripts.External.Components;
using App.Scripts.External.Localisation.MonoBehaviours;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.Features.LevelPacks
{
    public interface ILevelItemView : IGameObjectable, IClickable
    {
        TMP_Text GalacticPassedLevels { get; }
        Image GalacticPassedLevelsBackground { get; }
        Image BigBackground { get; }
        TMP_Text GalacticText { get; }
        UILocale GalacticName { get; }
        UILocale SubTextLocale { get; }
        Image Glow { get; }
        TMP_Text EnergyText { get; }
        Image LeftImageHalf { get; }
        Image GalacticIcon { get; }
        Image LockIcon { get; }
        Image MaskableImage { get; }
        GameObject EnergyPanel { get; }
        
        public class Factory : PlaceholderFactory<ILevelItemView>
        {
            
        }
    }
}