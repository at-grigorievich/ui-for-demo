using ATG.Activator;
using Cysharp.Threading.Tasks;

namespace ATG.UI
{
    public interface IUserInterfaceService: IActivateable
    {
        UIView GetViewByType(UIElementType type);
        UniTask<UIView> GetViewByTypeAsync(UIElementType type);
        
        UIView ShowViewByType(UIElementType type, object sender, object data);
        UniTask<UIView> ShowViewByTypeAsync(UIElementType type, object sender, object data, bool withOpennedDelay = false);

        void HideViewByType(UIElementType type);
        UniTask HideViewByTypeWithDelay(UIElementType type);

        void ChangeViewLayerIndexByType(UIElementType type, int layerIndex);
    }
}