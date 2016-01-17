using System;

namespace AppShell
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ViewAttribute : Attribute
    {
        public Type ViewModelType { get; private set; }

        public ViewAttribute(Type viewModelType)
        {
            ViewModelType = viewModelType;
        }
    }
}