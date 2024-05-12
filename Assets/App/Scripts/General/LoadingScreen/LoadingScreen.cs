using App.Scripts.General.LoadingScreen.Settings;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace App.Scripts.General.LoadingScreen
{
    public class LoadingScreen : MonoBehaviour, ILoadingScreen
    {
        [SerializeField] private RectTransform _rectTransform;
        
        private LoadingScreenSettings _loadingScreenSettings;
        private CanvasGroup _canvasGroup;

        [Inject]
        private void Construct(CanvasGroup canvasGroup, LoadingScreenSettings loadingScreenSettings)
        {
            _canvasGroup = canvasGroup;
            _loadingScreenSettings = loadingScreenSettings;
        }

        public RectTransform RectTransform => _rectTransform;

        public UniTask Show(bool showQuickly)
        {
            _canvasGroup.gameObject.SetActive(true);

            if (showQuickly)
            {
                _canvasGroup.alpha = 1f;
                return UniTask.CompletedTask;
            }
            
            return RunDotweenMachine(0f, 1f);
        }

        public async UniTask Hide()
        {
            await RunDotweenMachine(1f, 0f);
            _canvasGroup.gameObject.SetActive(false);
        }

        private UniTask RunDotweenMachine(float from, float to)
        {
            return DOVirtual.Float(from, to, _loadingScreenSettings.Duration, ChangeAlpha).ToUniTask();
        }

        private void ChangeAlpha(float value)
        {
            _canvasGroup.alpha = value;
        }
    }
}