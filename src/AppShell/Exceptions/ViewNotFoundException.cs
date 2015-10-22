using System;

namespace AppShell
{
    public class ViewNotFoundException : Exception
    {
        public ViewNotFoundException() : base("Could not find view.") { }
        public ViewNotFoundException(Type viewModelType) : base(string.Format("Could not find view for view model: {0}", viewModelType)) { }
        public ViewNotFoundException(string message) : base(message) { }
        public ViewNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
