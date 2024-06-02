using TMPro;
using UnityEngine.UI;

namespace App.Scripts.Scenes.GameScene.Features.Levels.LevelView
{
    public interface ILevelPackInfoView
    {
        TMP_Text PassedLevels { get; }
        TMP_Text LevelPassProgress { get; }
        Image GalacticIcon { get; }
    }
}