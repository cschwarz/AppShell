using AppShell.Data;
using AppShell.Data.Desktop;
using AppShell.Desktop;

namespace AppShell.Samples.Todo
{
    public class TodoApp : ShellApplication<TodoShellCore>
    {
        protected override void ConfigurePlatform()
        {
            base.ConfigurePlatform();

            ShellCore.Container.RegisterSingleton<ISQLiteDatabase, DesktopSQLiteDatabase>();
        }
    }
}
