using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace AppShell.Mobile
{
    public class ShellViewPage : ContentPage
    {
        public ShellViewPage()
        {
        }

        public static Page Create(object content)
        {
            if (content is Page)
                return content as Page;

            return new ShellViewPage() { Content = content as View };
        }
    }
}
