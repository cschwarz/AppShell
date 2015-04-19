using System;

namespace AppShell
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ViewAttribute : Attribute
    {
        public Type ViewModelType { get; private set; }

        public ViewAttribute(Type viewModelType)
        {
            ViewModelType = viewModelType;
        }
    }
}
