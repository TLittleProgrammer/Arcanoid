using App.Scripts.External.Localisation.MonoBehaviours;
using App.Scripts.General.Energy;
using App.Scripts.General.Popup;
using App.Scripts.Scenes.GameScene.Features.Dotween;
using App.Scripts.Scenes.GameScene.Features.Settings;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
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
        [SerializeField] private TMP_Text _winText;
        [SerializeField] private Transform _garbargePoint;
        [SerializeField] private Transform _buttonInitialPoint;

        private WinContinueButtonAnimationSettings _winContinueButtonAnimationSettings;
        private ITweenersLocator _tweenersLocator;
        
        [Inject]
        private void Construct(
            WinContinueButtonAnimationSettings winContinueButtonAnimationSettings,
            ITweenersLocator tweenersLocator)
        {
            _tweenersLocator = tweenersLocator;
            _winContinueButtonAnimationSettings = winContinueButtonAnimationSettings;
        }

        public UILocale GalacticName => _galacticName;
        public TMP_Text PassedLevelsText => _passedLevelsText;
        public Image BottomGalacticIcon => _bottomGalacticIcon;
        public Image TopGalacticIcon => _topGalacticIcon;
        public Button ContinueButton => _continueButton;
        public EnergyView EnergyView => _energyView;

        public override async UniTask Show()
        {
            Vector2 initialTextPosition = _winText.transform.position;
            Vector2 initialButtonPosition = _continueButton.transform.position;
            UpdatePositions();


            await transform.DOScale(Vector3.one, 0.5f).From(Vector3.zero);

            _textFalling.DOMoveY(_targetPositionForTextFalling.transform.position.y, 0.5f).SetEase(Ease.OutBounce).ToUniTask().Forget();
            
            _winText.transform.DOMove(initialTextPosition, 2f).SetEase(Ease.InOutElastic).SetDelay(0.25f).ToUniTask().Forget();
            _continueButton.transform.DOMove(initialButtonPosition, 2f).SetEase(Ease.InOutElastic).SetDelay(0.75f).ToUniTask().Forget();
            
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

        private void UpdatePositions()
        {
            _winText.transform.position = _garbargePoint.position;
            _continueButton.transform.position = _buttonInitialPoint.position;
        }

        public override UniTask Close()
        {
            _tweenersLocator.AddTweener(transform.DOScale(Vector3.zero, 1f).From(Vector3.zero));
            return UniTask.CompletedTask;
        }
    }
}