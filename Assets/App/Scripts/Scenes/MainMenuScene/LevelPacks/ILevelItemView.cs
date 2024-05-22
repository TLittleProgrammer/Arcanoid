using App.Scripts.External.Components;
using App.Scripts.External.Localisation.MonoBehaviours;
using App.Scripts.General.Levels;
using TMPro;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.LevelPacks
{
    public interface ILevelItemView : IGameObjectable, IClickable
    {
        TMP_Text GalacticPassedLevels { get; }
        Image GalacticPassedLevelsBackground { get; }
        Image BigBackground { get; }
        TMP_Text GalacticText { get; }
        UILocale GalacticName { get; }
        Image Glow { get; }
        TMP_Text EnergyText { get; }
        Image LeftImageHalf { get; }
        Image GalacticIcon { get; }
        Image LockIcon { get; }
        Image MaskableImage { get; }

        void UpdateVisual(LevelModel levelModel);
        
        public class Factory : PlaceholderFactory<int, LevelPack, ILevelItemView>
        {
            
        }
    }
}