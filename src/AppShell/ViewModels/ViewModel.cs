using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AppShell
{
    public class ViewModel : IViewModel
    {   
        private bool allowClose;
        public bool AllowClose
        {
            get { return allowClose; }
            set
            {
                if (allowClose != value)
                {
                    allowClose = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool hasNavigationBar;
        public bool HasNavigationBar
        {
            get { return hasNavigationBar; }
            set
            {
                if (hasNavigationBar != value)
                {
                    hasNavigationBar = value;
                    OnPropertyChanged();
                }
            }
        }
        
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    title = value;
                    OnPropertyChanged();
                }
            }
        }
        
        private string icon;
        public string Icon
        {
            get { return icon; }
            set
            {
                if (icon != value)
                {
                    icon = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            set
            {
                if (isLoading != value)
                {
                    isLoading = value;
                    OnPropertyChanged();
                }
            }
        }

        private string loadingText;
        public string LoadingText
        {
            get { return loadingText; }
            set
            {
                if (loadingText != value)
                {
                    loadingText = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<ToolbarItemViewModel> ToolbarItems { get; set; }

        public ViewModel()
        {
            AllowClose = true;
            HasNavigationBar = true;
            ToolbarItems = new ObservableCollection<ToolbarItemViewModel>();
        }

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
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
