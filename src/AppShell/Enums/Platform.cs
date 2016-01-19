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
        WinRT = 16,
        UWP = 32,
        Desktop = Windows | Mac,
        Mobile = Android | iOS | WinRT | UWP,
        All = Desktop | Mobile
    }
}
