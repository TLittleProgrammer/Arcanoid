using App.Scripts.Scenes.GameScene.LevelProgress;
using log4net.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.GameScene.LevelView
{
    public class LevelPackInfoView : MonoBehaviour, ILevelPackInfoView
    {
        [SerializeField] private TMP_Text _passedLevel;
        [SerializeField] private TMP_Text _levelPassProgress;
        [SerializeField] private Image _image;

        public TMP_Text PassedLevels => _passedLevel;
        public TMP_Text LevelPassProgress => _levelPassProgress;
        public Image Image => _image;
        
        public void UpdatePassedLevels(int currentLevel, int allLevels)
        {
            _passedLevel.text = $"{currentLevel}/{allLevels}";
        }

        public void UpdateProgressText(int value)
        {
            LevelPassProgress.text = $"{value}%";
        }

        public void Initialize(LevelPackInfoRecord packInfoRecord)
        {
            PassedLevels.text = $"{packInfoRecord.CurrentLevelIndex}/{packInfoRecord.AllLevelsCountFromPack}";
            Image.sprite = packInfoRecord.Sprite;
            
            UpdateProgressText(packInfoRecord.TargetScore);
        }
    }
}