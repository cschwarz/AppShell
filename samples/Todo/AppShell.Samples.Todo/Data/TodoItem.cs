using SQLite;
using System;

namespace AppShell.Samples.Todo
{
    public class TodoItem
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Text { get; set; }
    }
}
