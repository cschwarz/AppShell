using System;
using Xamarin.Forms;

namespace AppShell.Mobile
{
    public class MobileDataTemplateFactory : IDataTemplateFactory
    {
        public object GetDataTemplate(Type viewType)
        {
            return new DataTemplate(viewType);
        }
    }
}
