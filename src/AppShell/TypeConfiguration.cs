using System;
using System.Collections.Generic;

namespace AppShell
{
    public class TypeConfiguration
    {
        public Type Type { get; private set; }
        public Dictionary<string, object> Data { get; private set; }

        public TypeConfiguration(Type type)
            : this(type, null)
        {
        }

        public TypeConfiguration(Type type, Dictionary<string, object> data)
        {
            Type = type;
            Data = data;
        }
    }
}
