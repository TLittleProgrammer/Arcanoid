using App.Scripts.Scenes.GameScene.Features.Entities;
using Zenject.ReflectionBaking.Mono.CompilerServices.SymbolWriter;

namespace App.Scripts.Scenes.GameScene.Features.Blocks
{
    public interface IBlockShakeService
    {
        void Shake(IEntityView entityView);
    }
}