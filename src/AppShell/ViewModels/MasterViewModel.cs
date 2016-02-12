using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AppShell
{
    public abstract class MenuItem
    {
        public string Title { get; private set; }
        public string Icon { get; set; }

        public MenuItem(string title)
        {
            Title = title;
        }

        public abstract void Execute(IServiceDispatcher serviceDispatcher);
    }

    public class CommandMenuItem : MenuItem
    {
        public ICommand Command { get; private set; }

        public CommandMenuItem(string title, ICommand command)
            : base(title)
        {
            Command = command;
        }

        public override void Execute(IServiceDispatcher serviceDispatcher)
        {
            Command.Execute(null);
        }
    }

    public class ViewModelMenuItem : MenuItem
    {
        public TypeConfiguration TypeConfiguration { get; private set; }

        public ViewModelMenuItem(string title, TypeConfiguration typeConfiguration)
            : base(title)
        {
            TypeConfiguration = typeConfiguration;
        }

        public override void Execute(IServiceDispatcher serviceDispatcher)
        {
            serviceDispatcher.Dispatch<IMasterDetailNavigationService>(n => n.PushRoot(TypeConfiguration.Type, TypeConfiguration.Data));
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
                        selectedItem.Execute(serviceDispatcher);
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

        protected IServiceDispatcher serviceDispatcher;

        public MasterViewModel(IServiceDispatcher serviceDispatcher)
        {
            this.serviceDispatcher = serviceDispatcher;

            Title = "Menu";

            Items = new ObservableCollection<MenuItem>();
        }
    }
}
