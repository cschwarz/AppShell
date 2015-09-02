using System;

namespace AppShell
{
    [Flags]
    public enum Platform
    {
        None = 0,
        Windows = 1,
        Android = 2,
        iOS = 4,
        WindowsPhone = 8,
        Desktop = Windows,
        Mobile = Android | iOS | WindowsPhone,
        All = Desktop | Mobile
    }
}
