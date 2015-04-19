using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShell
{
    public interface IDataTemplateFactory
    {
        object GetDataTemplate(Type viewType);
    }
}
