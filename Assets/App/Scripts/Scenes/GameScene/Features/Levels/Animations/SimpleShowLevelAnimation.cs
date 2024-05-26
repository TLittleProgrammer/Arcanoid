using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.GameScene.Features.Constants;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Grid;
using App.Scripts.Scenes.GameScene.Features.Levels.Load;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.GameScene.Features.Levels.Animations
{
    public sealed class SimpleShowLevelAnimation : IShowLevelAnimation
    {
        private readonly ILevelLoader _levelLoader;
        private readonly IGridPositionResolver _gridPositionResolver;
        private readonly Image _menuButton;

        public SimpleShowLevelAnimation(
            ILevelLoader levelLoader,
            IGridPositionResolver gridPositionResolver,
            Image menuButton)
        {
            _levelLoader = levelLoader;
            _gridPositionResolver = gridPositionResolver;
            _menuButton = menuButton;
        }
        
        public async UniTask Show()
        {
            _menuButton.raycastTarget = false;
            
            float timeOffset = GameConstants.ShowLevelDuration / _levelLoader.Entities.Count;

            List<IEntityView> sortedList = _levelLoader.Entities.OrderBy(x => x.GridPositionY).ThenBy(x => x.GridPositionX).ToList();

            foreach (IEntityView view in sortedList)
            {
                view.GameObject.transform.DOScale(_gridPositionResolver.GetCellSize(), timeOffset).SetEase(Ease.InOutBounce).ToUniTask().Forget();
                await UniTask.Delay((int)(timeOffset * 1000));
            }

            _menuButton.raycastTarget = true;
            await UniTask.CompletedTask;
        }
    }
}