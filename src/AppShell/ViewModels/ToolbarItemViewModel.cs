using System.Windows.Input;

namespace AppShell
{
    public class ToolbarItemViewModel
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public ToolbarItemOrder Order { get; set; }
        public int Priority { get; set; }
        public ICommand Command { get; set; }
    }
}
