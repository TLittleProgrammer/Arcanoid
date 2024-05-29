using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.Helpers
{
    public interface IAnimatedDestroyService
    {
        UniTask Animate(List<EntityData> immediateEntityDatas);
        UniTask Animate(IEntityView immediateEntityDatas);
    }
}