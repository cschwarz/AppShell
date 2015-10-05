using System.ComponentModel;

namespace AppShell
{
    public interface IViewModel : INotifyPropertyChanged
    {
        bool AllowClose { get; set; }
        string Name { get; set; }
        string Title { get; set; }
    }
}
