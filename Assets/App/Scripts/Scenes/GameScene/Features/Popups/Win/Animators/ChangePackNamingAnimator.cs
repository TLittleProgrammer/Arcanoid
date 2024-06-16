using App.Scripts.External.Localisation.MonoBehaviours;
using App.Scripts.General.Animator;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Popups.Animators
{
    public class ChangePackNamingAnimator : MonoAnimator<string>
    {
        [SerializeField] private UILocale _galacticName;
        
        public override UniTask Animate(string localeKey)
        {
            DOVirtual.Float(1f, 0f, 1f, HideObjectsText).ToUniTask().Forget();

            _galacticName.SetToken(localeKey);

            DOVirtual.Float(0f, 1f, 1f, ShowObjectsText).ToUniTask().Forget();
            
            return UniTask.CompletedTask;
        }

        public override UniTask UndoAnimate()
        {
            return UniTask.CompletedTask;
        }
        
        private void ShowObjectsText(float value)
        {
            _galacticName.Text.color = new Color(1f, 1f, 1f, value);
        }

        private void HideObjectsText(float value)
        {
            _galacticName.Text.color = new Color(1f, 1f, 1f, value);
        }
    }
}