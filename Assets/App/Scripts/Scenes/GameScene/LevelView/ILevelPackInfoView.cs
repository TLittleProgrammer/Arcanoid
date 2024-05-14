using TMPro;
using UnityEngine.UI;

namespace App.Scripts.Scenes.GameScene.LevelView
{
    public interface ILevelPackInfoView
    {
        TMP_Text PassedLevels { get; }
        TMP_Text LevelPassProgress { get; }
        Image Image { get; }

        void UpdatePassedLevels(int currentLevel, int allLevels);
    }
}