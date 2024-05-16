using App.Scripts.External.Localisation.MonoBehaviours;
using App.Scripts.General.Components;
using App.Scripts.General.Levels;
using App.Scripts.Scenes.GameScene.Components;
using App.Scripts.Scenes.MainMenuScene.LevelPacks.Configs;
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
        Image LeftImageHalf { get; }
        Image GalacticIcon { get; }
        Image LockIcon { get; }
        Image MaskableImage { get; }

        void UpdateVisual(LevelItemViewData levelViewData, LevelPack levelPack, VisualTypeId visualTypeId, int passedLevels);
        
        public class Factory : PlaceholderFactory<int, LevelPack, ILevelItemView>
        {
            
        }
    }
}