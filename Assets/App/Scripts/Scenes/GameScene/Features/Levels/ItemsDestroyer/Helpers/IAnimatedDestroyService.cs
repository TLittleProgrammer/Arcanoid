using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Entities;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.Helpers
{
    public interface IAnimatedDestroyService
    {
        UniTask Animate(List<EntityData> immediateEntityDatas);
        UniTask Animate(IEntityView immediateEntityDatas);
    }
}