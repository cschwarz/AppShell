using System;

namespace AppShell
{
    public interface IDataTemplateFactory
    {
        object GetDataTemplate(Type viewType);
    }
}
