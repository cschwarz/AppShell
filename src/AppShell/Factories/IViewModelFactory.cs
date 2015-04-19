﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShell
{
    public interface IViewModelFactory
    {
        IViewModel GetViewModel(Type viewModelType, Dictionary<string, object> data = null);
    }
}
