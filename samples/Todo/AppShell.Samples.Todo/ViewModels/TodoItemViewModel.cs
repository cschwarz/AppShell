using System;

namespace AppShell.Samples.Todo
{
    public class TodoItemViewModel : ViewModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        
        public TodoItemViewModel(TodoItem item)
        {
            Id = item.Id;
            Text = item.Text;
        }
    }
}
