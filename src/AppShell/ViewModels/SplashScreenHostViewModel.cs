using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShell
{
    public class SplashScreenHostViewModel : ViewModel
    {
        public event EventHandler ExitRequested;
        public event EventHandler ShellViewRequested;

        private object contentTemplate;
        public object ContentTemplate
        {
            get { return contentTemplate; }
            set
            {
                if (contentTemplate != value)
                {
                    contentTemplate = value;
                    OnPropertyChanged("ContentTemplate");
                }
            }
        }

        private IViewModel content;
        public IViewModel Content
        {
            get { return content; }
            set
            {
                if (content != value)
                {
                    content = value;
                    OnPropertyChanged("Content");
                }
            }
        }

        protected class SplashScreenContainer
        {
            public SplashScreenViewModel ViewModel { get; private set; }
            public Type ViewType { get; private set; }

            public SplashScreenContainer(SplashScreenViewModel viewModel, Type viewType)
            {
                ViewModel = viewModel;
                ViewType = viewType;
            }
        }

        protected SplashScreenContainer currentSplashScreen;
        protected Stack<SplashScreenContainer> splashScreens;

        protected IShellConfigurationProvider configurationProvider;
        protected IViewFactory viewFactory;
        protected IViewModelFactory viewModelFactory;
        protected IDataTemplateFactory dataTemplateFactory;

        public SplashScreenHostViewModel(IShellConfigurationProvider configurationProvider, IViewFactory viewFactory, IViewModelFactory viewModelFactory, IDataTemplateFactory dataTemplateFactory)
        {
            this.configurationProvider = configurationProvider;
            this.viewFactory = viewFactory;
            this.viewModelFactory = viewModelFactory;
            this.dataTemplateFactory = dataTemplateFactory;

            splashScreens = new Stack<SplashScreenContainer>();
            
            foreach (TypeConfiguration splashScreenType in configurationProvider.GetSplashScreens().Reverse())
            {
                SplashScreenViewModel viewModel = viewModelFactory.GetViewModel(splashScreenType.Type, splashScreenType.Data) as SplashScreenViewModel;
                Type viewType = viewFactory.GetViewType(splashScreenType.Type);

                splashScreens.Push(new SplashScreenContainer(viewModel, viewType));
            }
        }

        public virtual void ShowSplashScreens()
        {
            ShowNextSplashScreen();
        }

        protected virtual void ShowNextSplashScreen()
        {
            if (!splashScreens.Any())
            {
                ShowMainView();
                return;
            }

            currentSplashScreen = splashScreens.Pop();

            currentSplashScreen.ViewModel.Closed += SplashScreen_Closed;

            ContentTemplate = dataTemplateFactory.GetDataTemplate(currentSplashScreen.ViewType);
            Content = currentSplashScreen.ViewModel;
        }

        protected virtual void ShowMainView()
        {
            if (ShellViewRequested != null)
                ShellViewRequested(this, EventArgs.Empty);
        }

        private void SplashScreen_Closed(object sender, SplashScreenEventArgs e)
        {
            currentSplashScreen.ViewModel.Closed -= SplashScreen_Closed;
                        
            if (!e.Result && ExitRequested != null)
                ExitRequested(sender, EventArgs.Empty);
            if (e.Result)
                ShowNextSplashScreen();
        }
    }
}
