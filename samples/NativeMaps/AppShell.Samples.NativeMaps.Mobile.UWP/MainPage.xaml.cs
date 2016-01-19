using AppShell.Mobile;
using AppShell.Mobile.UWP;
using AppShell.NativeMaps.Mobile.UWP;

namespace AppShell.Samples.NativeMaps.Mobile.UWP
{
    public class NativeMapsShellPage : ShellPage<ShellApplication<NativeMapsShellCore>>
    {
    }

    public sealed partial class MainPage : NativeMapsShellPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            MapViewRenderer.ApiKey = "cqCo1cfFgtjjwz2LX1mC~T-RrbLstKjU-MfedDTdxVA~AjEW5pD0EeNYilFetOkJuuyGgEy5hWxZ9072Oeo_cPjg_desmyRS6eQwP48Ks0qc";

            Init();
        }
    }
}
