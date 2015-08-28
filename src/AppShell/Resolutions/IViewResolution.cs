using System;
using System.Collections.Generic;

namespace AppShell
{
    public interface IViewResolution
    {
        IDictionary<Type, Type> GetViewMapping(IEnumerable<Type> types);
    }
}
