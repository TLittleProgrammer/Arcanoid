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
    }
}