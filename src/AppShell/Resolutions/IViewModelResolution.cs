using System;
using System.Collections.Generic;

namespace AppShell
{
    public interface IViewModelResolution
    {
        IDictionary<Type, Type> GetViewModelMapping(IEnumerable<Type> types);
    }
}
