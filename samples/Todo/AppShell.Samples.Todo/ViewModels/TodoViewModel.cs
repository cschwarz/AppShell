using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShell.Samples.Todo
{
    public class TodoViewModel : ViewModel
    {
        public ObservableCollection<string> Items { get; private set; }

        private string todoItem;
        public string TodoItem
        {
            get { return todoItem; }
            set
            {
                if (todoItem != value)
                {
                    todoItem = value;
                    OnPropertyChanged();
                    AddItemCommand.ChangeCanExecute();
                }
            }
        }

        public Command AddItemCommand { get; private set; }

        public TodoViewModel()
        {
            AllowClose = false;
            AddItemCommand = new Command(AddItem, CanAddItem);

            Items = new ObservableCollection<string>();
        }

        public void AddItem()
        {
            Items.Add(TodoItem);
            TodoItem = string.Empty;
        }

        public bool CanAddItem()
        {
            return !string.IsNullOrEmpty(TodoItem);
        }
    }
}
