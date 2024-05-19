using App.Scripts.External.Components;
using Cysharp.Threading.Tasks;

namespace App.Scripts.General.LoadingScreen
{
    public interface ILoadingScreen : IRectTransformable
    {
        UniTask Show(bool showQuickly);
        UniTask Hide();
    }
}