using System.Threading.Tasks;
using App.Scripts.External.Initialization;
using App.Scripts.General.Components;
using App.Scripts.General.Popup.Factory;
using App.Scripts.Scenes.GameScene.Components;
using Cysharp.Threading.Tasks;
using Zenject;

namespace App.Scripts.General.Popup
{
    public interface IViewPopupProvider : IGameObjectable
    {
        PopupTypeId PopupTypeId { get; set; }
        
        UniTask Show();
        UniTask Close();

        void LockButtons();
        void UnlockButtons();
        
        public class Factory : PlaceholderFactory<PopupTypeId, ITransformable, IViewPopupProvider>, IAsyncInitializable<DiContainer>
        {
            public async UniTask AsyncInitialize(DiContainer param)
            {
                await UniTask.CompletedTask;
            }
        }
    }
}