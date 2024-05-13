using App.Scripts.General.LoadingScreen.Settings;
using App.Scripts.General.RootUI;
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
        private RootUIViewProvider _rootUiViewProvider;

        [Inject]
        private void Construct(RootUIViewProvider rootUIViewProvider, LoadingScreenSettings loadingScreenSettings)
        {
            _rootUiViewProvider = rootUIViewProvider;
            _loadingScreenSettings = loadingScreenSettings;
        }

        public RectTransform RectTransform => _rectTransform;

        public UniTask Show(bool showQuickly)
        {
            _rootUiViewProvider.LoadingCanvasGroup.blocksRaycasts = true;
            _rootUiViewProvider.gameObject.SetActive(true);

            if (showQuickly)
            {
                _rootUiViewProvider.LoadingCanvasGroup.alpha = 1f;
                return UniTask.CompletedTask;
            }
            
            return RunDotweenMachine(0f, 1f);
        }

        public async UniTask Hide()
        {
            await RunDotweenMachine(1f, 0f);
            _rootUiViewProvider.LoadingCanvasGroup.blocksRaycasts = true;
        }

        private UniTask RunDotweenMachine(float from, float to)
        {
            return DOVirtual.Float(from, to, _loadingScreenSettings.Duration, ChangeAlpha).ToUniTask();
        }

        private void ChangeAlpha(float value)
        {
            _rootUiViewProvider.LoadingCanvasGroup.alpha = value;
        }
    }
}