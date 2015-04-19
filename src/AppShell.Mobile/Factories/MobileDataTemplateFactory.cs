using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
