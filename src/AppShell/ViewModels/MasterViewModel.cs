using System.Collections.ObjectModel;

namespace AppShell
{
    public class MenuItem
    {
        public string Title { get; private set; }
        public TypeConfiguration TypeConfiguration { get; private set; }

        public MenuItem(string title, TypeConfiguration typeConfiguration)
        {
            Title = title;
            TypeConfiguration = typeConfiguration;
        }
    }

    public class MasterViewModel : ViewModel
    {
        public ObservableCollection<MenuItem> Items { get; private set; }

        private MenuItem selectedItem;
        public MenuItem SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;

                    if (selectedItem != null)
                    {
                        serviceDispatcher.Dispatch<IMasterDetailNavigationService>(n => n.PushRoot(selectedItem.TypeConfiguration.Type, selectedItem.TypeConfiguration.Data));
                        IsPresented = false;
                    }

                    OnPropertyChanged();
                }
            }
        }

        private bool isPresented;
        public bool IsPresented
        {
            get { return isPresented; }
            set
            {
                if (isPresented != value)
                {
                    isPresented = value;
                    OnPropertyChanged();
                }
            }
        }

        private IServiceDispatcher serviceDispatcher;

        public MasterViewModel(IServiceDispatcher serviceDispatcher)
        {
            this.serviceDispatcher = serviceDispatcher;

            Title = "Menu";

            Items = new ObservableCollection<MenuItem>();
        }
    }    
}
