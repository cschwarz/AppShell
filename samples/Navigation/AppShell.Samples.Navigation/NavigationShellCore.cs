﻿using AppShell;

namespace AppShell.Samples.Navigation
{
    public class NavigationShellCore : ShellCore
    {
        public override void Configure()
        {
            base.Configure();

            Container.RegisterSingleton<IShellConfigurationProvider, NavigationShellConfigurationProvider>();
        }
    }
}