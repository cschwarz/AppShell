using System;
using System.ComponentModel;

namespace AppShell
{
    public class ViewModel : IViewModel
    {
        public bool AllowClose { get; set; }
        public bool HasNavigationBar { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }

        public ViewModel()
        {
            AllowClose = true;
            HasNavigationBar = true;
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void Dispose()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
