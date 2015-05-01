﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShell.Samples.ServiceDispatcher
{
    public class ServiceDispatcherAppShellCore : AppShellCore
    {
        public override void Configure()
        {
            base.Configure();

            Container.RegisterSingle<IShellConfigurationProvider, ServiceDispatcherShellConfigurationProvider>();
        }
    }
}