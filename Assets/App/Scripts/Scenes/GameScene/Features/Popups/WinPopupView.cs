using App.Scripts.External.Localisation.MonoBehaviours;
using App.Scripts.General.Energy;
using App.Scripts.General.Popup;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace App.Scripts.Scenes.GameScene.Features.Popups
{
    public class WinPopupView : PopupView, IWinPopupView
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private UILocale _galacticName;
        [SerializeField] private Image _bottomGalacticIcon;
        [SerializeField] private Image _topGalacticIcon;
        [SerializeField] private TMP_Text _passedLevelsText;
        [SerializeField] private RectTransform _textFalling;
        [SerializeField] private RectTransform _targetPositionForTextFalling;
        [SerializeField] private EnergyView _energyView;
        [SerializeField] private Transform _circleEffect;

        public UILocale GalacticName => _galacticName;
        public TMP_Text PassedLevelsText => _passedLevelsText;
        public Image BottomGalacticIcon => _bottomGalacticIcon;
        public Image TopGalacticIcon => _topGalacticIcon;
        public Button ContinueButton => _continueButton;
        public RectTransform TextFalling => _textFalling;
        public RectTransform TargetPositionForTextFalling => _targetPositionForTextFalling;
        public Transform CircleEffect => _circleEffect;
        public EnergyView EnergyView => _energyView;
        public Transform Transform => transform;

        public override UniTask Show()
        {
            return UniTask.CompletedTask;
        }
    }
}