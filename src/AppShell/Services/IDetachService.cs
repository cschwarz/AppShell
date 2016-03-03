using System;
using System.Collections.Generic;

namespace AppShell
{
    [Service("detachService")]
    public interface IDetachService : IService
    {
        [ServiceMethod("detachActive")]
        void DetachActive();
        void PushDetached<TViewModel>(Dictionary<string, object> data = null) where TViewModel : class, IViewModel;
        void PushDetached<TViewModel>(dynamic data) where TViewModel : class, IViewModel;
        void PushDetached(Type viewModelType, Dictionary<string, object> data = null);
        [ServiceMethod("pushDetached")]
        void PushDetached(string viewModelType, Dictionary<string, object> data = null);
        void CloseDetached(IViewModel viewModel);
        [ServiceMethod("closeDetached")]
        void CloseDetached(string name);
        [ServiceMethod("activateDetached")]
        void ActivateDetached(string name);
        [ServiceMethod("detachedExists")]
        bool DetachedExists(string name);
    }
}
