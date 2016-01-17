namespace AppShell.Samples.Todo
{
    public class TodoShellCore : ShellCore
    {
        public override void Run()
        {
            Push<StackShellViewModel>(new { Name = "Main" });

            serviceDispatcher.Dispatch<INavigationService>(n => n.Push<TodoViewModel>());
        }
    }
}
