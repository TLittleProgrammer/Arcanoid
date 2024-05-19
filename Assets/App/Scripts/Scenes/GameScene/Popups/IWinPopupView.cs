using App.Scripts.External.Localisation.MonoBehaviours;
using TMPro;
using UnityEngine.UI;

namespace App.Scripts.Scenes.GameScene.Popups
{
    public interface IWinPopupView
    {
        UILocale GalacticName { get; }
        TMP_Text PassedLevelsText { get; }
        Image BottomGalacticIcon { get; }
        Button ContinueButton { get; }
    }
}