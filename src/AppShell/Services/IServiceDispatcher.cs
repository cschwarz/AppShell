using System;
using System.Collections.Generic;

namespace AppShell
{
    public interface IServiceDispatcher
    {
        IDictionary<string, IEnumerable<string>> Services { get; }
        
        void Initialize();
        
        void Subscribe<T>(T service);
        void Unsubscribe<T>(T service);

        void Dispatch<T>(Action<T> predicate);
        void Dispatch(string serviceName, string methodName, object[] parameters);
    }
}
