using App.Scripts.External.Components;
using Cysharp.Threading.Tasks;

namespace App.Scripts.General.Popup
{
    public interface IPopupView : IGameObjectable
    { 
        UniTask Show();
        UniTask Close();
    }
}