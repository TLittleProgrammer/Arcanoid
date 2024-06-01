using App.Scripts.External.Components;
using App.Scripts.External.Localisation.MonoBehaviours;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.GameScene.Features.Popups
{
    public interface IWinPopupView : ITransformable
    {
        UILocale GalacticName { get; }
        TMP_Text PassedLevelsText { get; }
        Image BottomGalacticIcon { get; }
        Button ContinueButton { get; }
        RectTransform TextFalling { get; }
        RectTransform TargetPositionForTextFalling { get; }
        Transform CircleEffect { get; }
    }
}