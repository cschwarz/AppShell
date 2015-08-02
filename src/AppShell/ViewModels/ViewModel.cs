using System.ComponentModel;

namespace AppShell
{
    public class ViewModel : IViewModel
    {
        public bool AllowClose { get; set; }
        public string Title { get; set; }

        public ViewModel()
        {
            AllowClose = true;
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
