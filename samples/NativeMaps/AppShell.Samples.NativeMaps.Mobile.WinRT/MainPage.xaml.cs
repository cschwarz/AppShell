using AppShell.Mobile;
using AppShell.Mobile.WinRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
