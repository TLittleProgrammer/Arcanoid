using Cysharp.Threading.Tasks;

namespace External.Initialization
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