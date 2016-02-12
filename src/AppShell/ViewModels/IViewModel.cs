using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AppShell
{
    public interface IViewModel : INotifyPropertyChanged, IDisposable
    {
        bool AllowClose { get; set; }
        bool HasNavigationBar { get; set; }
        string Name { get; set; }
        string Title { get; set; }
        string Icon { get; set; }
        bool IsLoading { get; set; }
        string LoadingText { get; set; }
        ObservableCollection<ToolbarItemViewModel> ToolbarItems { get; set; }
        void OnActivated();
    }
}
