using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;

namespace AppShell.Desktop
{
    /// <summary>
    /// Interaction logic for ShellWindow.xaml
    /// </summary>
    public partial class ShellWindow : Window
    {
        private Dictionary<IViewModel, Window> detachedWindows;

        public ShellWindow()
        {
            InitializeComponent();

            detachedWindows = new Dictionary<IViewModel, Window>();

            DataContextChanged += ShellWindow_DataContextChanged;
        }

        private void ShellWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                ShellViewModel oldShellViewModel = e.OldValue as ShellViewModel;

                oldShellViewModel.ActivateDetachedRequested -= ShellViewModel_ActivateDetachedRequested;
                oldShellViewModel.CloseRequested -= ShellViewModel_CloseRequested;
                oldShellViewModel.DetachedItems.CollectionChanged -= DetachedItems_CollectionChanged;

            }
            if (e.NewValue != null)
            {
                ShellViewModel newShellViewModel = e.NewValue as ShellViewModel;

                newShellViewModel.ActivateDetachedRequested += ShellViewModel_ActivateDetachedRequested;
                newShellViewModel.CloseRequested += ShellViewModel_CloseRequested;
                newShellViewModel.DetachedItems.CollectionChanged += DetachedItems_CollectionChanged;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            foreach (var detachedPair in detachedWindows.ToDictionary(k => k.Key, v => v.Value))
                (DataContext as ShellViewModel).CloseDetached(detachedPair.Key);
        }

        private void DetachedItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (IViewModel viewModel in e.NewItems)
                {
                    DetachedWindow detachedWindow = new DetachedWindow(DataContext as ShellViewModel);
                    detachedWindow.DataContext = viewModel;
                    detachedWindow.Show();

                    detachedWindows.Add(viewModel, detachedWindow);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (IViewModel viewModel in e.OldItems)
                {
                    if (detachedWindows.ContainsKey(viewModel))
                    {
                        detachedWindows[viewModel].Close();
                        detachedWindows.Remove(viewModel);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                foreach (var detachedWindow in detachedWindows)
                    detachedWindow.Value.Close();

                detachedWindows.Clear();
            }
        }

        private void ShellViewModel_ActivateDetachedRequested(object sender, IViewModel e)
        {
            if (detachedWindows.ContainsKey(e))
                detachedWindows[e].Activate();
        }

        private void ShellViewModel_CloseRequested(object sender, EventArgs e)
        {
            Close();
        }
    }
}
