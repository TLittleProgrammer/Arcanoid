using Cysharp.Threading.Tasks;

namespace General.Initialization
{
    public interface IAsyncInitializable
    {
        UniTask AsyncInitialize();
    }
    
    public interface IAsyncInitializable<TFirstParam>
    {
        UniTask AsyncInitialize(TFirstParam param);
    }
}