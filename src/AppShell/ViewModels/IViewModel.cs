﻿using System;
using System.ComponentModel;

namespace AppShell
{
    public interface IViewModel : INotifyPropertyChanged, IDisposable
    {
        bool AllowClose { get; set; }
        string Name { get; set; }
        string Title { get; set; }
    }
}
