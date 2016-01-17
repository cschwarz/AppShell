using System;
using Xamarin.Forms;

namespace AppShell.Mobile
{
    public interface IPageReady
    {
        event EventHandler<Page> Ready;
        bool IsReady { get; }
    }
}
