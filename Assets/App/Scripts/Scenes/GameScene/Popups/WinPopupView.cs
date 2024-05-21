using App.Scripts.External.GameStateMachine;
using App.Scripts.External.Localisation.MonoBehaviours;
using App.Scripts.General.Energy;
using App.Scripts.General.Popup;
using App.Scripts.Scenes.GameScene.Dotween;
using App.Scripts.Scenes.GameScene.Settings;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Button = UnityEngine.UI.Button;

namespace App.Scripts.Scenes.GameScene.Popups
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

        private WinContinueButtonAnimationSettings _winContinueButtonAnimationSettings;
        private ITweenersLocator _tweenersLocator;
        private Button _continue;
        
        [Inject]
        private void Construct(
            WinContinueButtonAnimationSettings winContinueButtonAnimationSettings,
            ITweenersLocator tweenersLocator)
        {
            _tweenersLocator = tweenersLocator;
            _winContinueButtonAnimationSettings = winContinueButtonAnimationSettings;

            Buttons = new()
            {
                _continueButton
            };
        }

        public UILocale GalacticName => _galacticName;
        public TMP_Text PassedLevelsText => _passedLevelsText;
        public Image BottomGalacticIcon => _bottomGalacticIcon;
        public Image TopGalacticIcon => _topGalacticIcon;
        public Button ContinueButton => _continueButton;
        public EnergyView EnergyView => _energyView;

        public override async UniTask Show()
        {
            await transform.DOScale(Vector3.one, 1f).From(Vector3.zero);

            _textFalling.DOMoveY(_targetPositionForTextFalling.transform.position.y, 0.5f).SetEase(Ease.OutBounce).ToUniTask().Forget();

            Sequence sequence = DOTween.Sequence();

            sequence.Append(
                _continueButton
                    .transform
                    .DOScale(_winContinueButtonAnimationSettings.TargetScale,
                        _winContinueButtonAnimationSettings.Duration)
                    .SetDelay(_winContinueButtonAnimationSettings.Delay)
                    .SetLoops(2, LoopType.Yoyo))
                .SetEase(_winContinueButtonAnimationSettings.Ease)
                .SetLoops(-1, LoopType.Restart)
                .ToUniTask().Forget();


            _tweenersLocator.AddSequence(sequence);
        }
    }
}