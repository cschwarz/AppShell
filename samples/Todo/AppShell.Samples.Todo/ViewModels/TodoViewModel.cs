using AppShell.Data;
using SQLite;
using System;
using System.Collections.ObjectModel;

namespace AppShell.Samples.Todo
{
    public class TodoViewModel : ViewModel
    {
        public ObservableCollection<TodoItemViewModel> Items { get; private set; }

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

        protected ISQLiteDatabase database;
        protected SQLiteConnection connection;

        public TodoViewModel(ISQLiteDatabase database)
        {
            this.database = database;

            AllowClose = false;
            AddItemCommand = new Command(AddItem, CanAddItem);

            Items = new ObservableCollection<TodoItemViewModel>();

            connection = database.GetConnection("TodoApp/Todo.db");
            connection.CreateTable<TodoItem>();

            foreach (TodoItem item in connection.Query<TodoItem>("SELECT * FROM TodoItem"))
                Items.Add(new TodoItemViewModel(item));
        }

        public void AddItem()
        {
            TodoItem todoItem = new TodoItem();
            todoItem.Id = Guid.NewGuid();
            todoItem.Text = TodoItem;

            connection.Insert(todoItem);

            Items.Add(new TodoItemViewModel(todoItem));
            TodoItem = string.Empty;
        }

        public bool CanAddItem()
        {
            return !string.IsNullOrEmpty(TodoItem);
        }
    }
}
