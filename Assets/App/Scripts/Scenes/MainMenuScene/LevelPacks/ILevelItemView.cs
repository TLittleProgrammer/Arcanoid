using App.Scripts.External.Localisation.MonoBehaviours;
using App.Scripts.General.Components;
using App.Scripts.Scenes.GameScene.Components;
using App.Scripts.Scenes.MainMenuScene.LevelPacks.Configs;
using TMPro;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.LevelPacks
{
    public interface ILevelItemView : IGameObjectable
    {
        TMP_Text GalacticPassedLevels { get; }
        Image GalacticPassedLevelsBackground { get; }
        Image BigBackground { get; }
        UILocale GalacticName { get; }
        Image Glow { get; }
        Image LeftImageHalf { get; }
        Image GalacticIcon { get; }
        Image MaskableImage { get; }
        
        public class Factory : PlaceholderFactory<int, ILevelItemView, ITransformable, LevelPack, ILevelItemView>
        {
            
        }
    }
}