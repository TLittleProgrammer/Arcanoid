using System.Collections.Generic;
using App.Scripts.External.Localisation.MonoBehaviours;
using App.Scripts.General.Animator;
using App.Scripts.General.Command;
using App.Scripts.General.Components;
using App.Scripts.General.Energy;
using App.Scripts.General.Levels;
using App.Scripts.General.MVVM.Energy;
using App.Scripts.General.Popup;
using App.Scripts.Scenes.GameScene.Command.Interfaces;
using App.Scripts.Scenes.GameScene.Command.Win;
using App.Scripts.Scenes.GameScene.Features.Dotween;
using App.Scripts.Scenes.GameScene.MVVM.Popups.Win;
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
        [SerializeField] private Transform _packViewTransform;
        
        [SerializeField] private MonoAnimator _changeIconPositions;
        [SerializeField] private MonoAnimator<ITweenersLocator> _showWinAnimator;
        [SerializeField] private MonoAnimator<ITweenersLocator> _circleAnimator;
        [SerializeField] private MonoAnimator<string> _changePackNamingAnimator;
        [SerializeField] private MonoAnimator<LevelPack, LevelPack> _changeLevelTextAnimator;

        private IDisableButtonsCommand _disableButtonsCommand;
        private WinViewModel _winViewModel;
        private ILoadNextLevelCommand _loadNextLevelCommand;
        private ITweenersLocator _tweenersLocator;

        public UILocale GalacticName => _galacticName;
        public TMP_Text PassedLevelsText => _passedLevelsText;
        public Image BottomGalacticIcon => _bottomGalacticIcon;
        public Image TopGalacticIcon => _topGalacticIcon;
        public Button ContinueButton => _continueButton;
        public RectTransform TextFalling => _textFalling;
        public RectTransform TargetPositionForTextFalling => _targetPositionForTextFalling;
        public Transform CircleEffect => _circleEffect;
        public Transform PackViewTransform => _packViewTransform;
        public EnergyView EnergyView => _energyView;
        public Transform Transform => transform;

        public void Initialize(WinViewModel winViewModel,
            IDisableButtonsCommand disableButtonsCommand,
            ILoadNextLevelCommand loadNextLevelCommand,
            ITweenersLocator tweenersLocator,
            EnergyViewModel energyViewModel)
        {
            _tweenersLocator = tweenersLocator;
            _loadNextLevelCommand = loadNextLevelCommand;
            _winViewModel = winViewModel;
            _disableButtonsCommand = disableButtonsCommand;
            _energyView.Initialize(energyViewModel);
            
            
            _continueButton.onClick.AddListener(OnContinueClicked);

            InitializeVisual();
        }

        private void InitializeVisual()
        {
            WinViewRecord record = _winViewModel.GetViewRecord();

            BottomGalacticIcon.sprite = record.Sprite;
            TopGalacticIcon.sprite = record.TopGalacticSprite;
            GalacticName.SetToken(record.Token);
            PassedLevelsText.text = $"{record.CurrentLevel}/{record.AllLevelsCount}";
        }

        public override async UniTask Show()
        {
            await _circleAnimator.Animate(_tweenersLocator);
            await _showWinAnimator.Animate(_tweenersLocator);

            AnimateNextLevelPackIfNeed();
        }

        private async void AnimateNextLevelPackIfNeed()
        {
            if (_winViewModel.NeedLoadNextPack())
            {
                _continueButton.interactable = false;
                
                await _changePackNamingAnimator.Animate(_winViewModel.GetNextLevelPack().LocaleKey);
                _changeLevelTextAnimator.Animate(_winViewModel.GetCurrentLevelPack(), _winViewModel.GetNextLevelPack()).Forget();
                await _changeIconPositions.Animate();
                _continueButton.interactable = true;
            }
        }

        private void OnContinueClicked()
        {
            ExecuteCommand(_loadNextLevelCommand);
        }

        private void ExecuteCommand(ICommand command)
        {
            List<Button> buttons = new()
            {
                _continueButton
            };
            
            _disableButtonsCommand.Execute(buttons);
            
            command.Execute();
        }
    }
}