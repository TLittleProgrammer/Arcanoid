using Cysharp.Threading.Tasks;

namespace App.Scripts.External.Initialization
{
    public interface IAsyncInitializable
    {
        UniTask AsyncInitialize();
    }
    
    public interface IAsyncInitializable<TFirstParam>
    {
        UniTask AsyncInitialize(TFirstParam param);
    }
    
    public interface IAsyncInitializable<TFirstParam, TSecondParam>
    {
        UniTask AsyncInitialize(TFirstParam param, TSecondParam secondParam);
    }
}