using System.ComponentModel;

namespace AppShell
{
    public interface IViewModel : INotifyPropertyChanged
    {
        bool AllowClose { get; set; }
    }
}
