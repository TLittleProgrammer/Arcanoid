using System.Collections.Generic;
using App.Scripts.External.Components;
using App.Scripts.General.Infrastructure;
using App.Scripts.General.Popup.Factory;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.General.Popup
{
    public sealed class PopupService : IPopupService, IGeneralRestartable
    {
        private readonly IPopupFactory _factory;
        private readonly IBackPopupPlane _backPopupPlane;
        private readonly ITransformable _defaultParent;
        private readonly List<IPopupView> _popupsList;

        public PopupService(
            IPopupFactory factory,
            IBackPopupPlane backPopupPlane,
            ITransformable defaultParent)
        {
            _factory = factory;
            _backPopupPlane = backPopupPlane;
            _defaultParent = defaultParent;
            _popupsList = new();
        }

        public TPopupView Show<TPopupView>(ITransformable parent = null) where TPopupView : IPopupView
        {
            ITransformable choosedParent = parent ?? _defaultParent;
            IPopupView popupView = _factory.Create<TPopupView>();
            
            popupView.GameObject.transform.SetParent(choosedParent.Transform, false);
            
            UpdateRaycastTargetForBackPanel(true);
            UpdateSiblingPosition();
            
            _popupsList.Add(popupView);
            popupView.Show();
            
            return (TPopupView)popupView;
        }

        public async UniTask Close<TPopup>() where TPopup : IPopupView
        {
            IPopupView popupViewProvider = FindPopup<TPopup>();

            if (popupViewProvider is not null)
            {
                await popupViewProvider.Close();
                
                _popupsList.Remove(popupViewProvider);
                Object.Destroy(popupViewProvider.GameObject);

                if (_popupsList.Count != 0)
                {
                    UpdateRaycastTargetForBackPanel(true);
                    UpdateSiblingPosition();
                }
                else
                {
                    UpdateRaycastTargetForBackPanel(false);
                }
            }

            await UniTask.CompletedTask;
        }

        public UniTask CloseAll()
        {
            foreach (IPopupView view in _popupsList)
            {
                Object.Destroy(view.GameObject);
            }
            
            UpdateRaycastTargetForBackPanel(false);
            _popupsList.Clear();
            
            return UniTask.CompletedTask;
        }

        public void Restart()
        {
            foreach (IPopupView viewPopupProvider in _popupsList)
            {
                Object.Destroy(viewPopupProvider.GameObject);
            }

            UpdateRaycastTargetForBackPanel(false);
            _popupsList.Clear();
        }

        private IPopupView FindPopup<TPopup>() where TPopup : IPopupView
        {
            foreach (IPopupView provider in _popupsList)
            {
                if (provider is TPopup)
                {
                    return provider;
                }
            }

            return null;
        }

        private void UpdateSiblingPosition()
        {
            _backPopupPlane.Transform.SetSiblingIndex(_popupsList.Count + 1);
        }

        private void UpdateRaycastTargetForBackPanel(bool value)
        {
            _backPopupPlane.Image.raycastTarget = value;
        }
    }
}