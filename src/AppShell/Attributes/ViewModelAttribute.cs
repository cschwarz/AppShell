using System;

namespace AppShell
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ViewModelAttribute : Attribute
    {
        public Type Substitute { get; set; }
    }
}
