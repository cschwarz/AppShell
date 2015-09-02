using System;

namespace AppShell
{
    [Flags]
    public enum Platform
    {
        None = 0,
        Windows = 1,
        Mac = 2,
        Android = 4,
        iOS = 8,
        WindowsPhone = 16,
        Desktop = Windows | Mac,
        Mobile = Android | iOS | WindowsPhone,
        All = Desktop | Mobile
    }
}
