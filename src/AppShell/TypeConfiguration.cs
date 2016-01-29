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

        public static TypeConfiguration Create<T>()
        {
            return new TypeConfiguration(typeof(T));
        }

        public static TypeConfiguration Create<T>(dynamic data)
        {
            return new TypeConfiguration(typeof(T), ObjectExtensions.ToDictionary(data));
        }

        public static TypeConfiguration Create(Type type, dynamic data)
        {
            return new TypeConfiguration(type, ObjectExtensions.ToDictionary(data));
        }
    }
}
