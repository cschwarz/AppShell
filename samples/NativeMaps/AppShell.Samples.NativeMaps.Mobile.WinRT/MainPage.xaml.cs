using AppShell.Mobile;
using AppShell.Mobile.WinRT;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace AppShell.Samples.NativeMaps.Mobile.WinRT
{
    public class NativeMapsShellPage : ShellPage<ShellApplication<NativeMapsShellCore>>
    {
    }

    public sealed partial class MainPage : NativeMapsShellPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            Init();
        }
    }
}
