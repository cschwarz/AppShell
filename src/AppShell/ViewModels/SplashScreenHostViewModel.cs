using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShell
{
    /*
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
            public TypeConfiguration ViewModelType { get; private set; }
            public Type ViewType { get; private set; }

            public SplashScreenContainer(TypeConfiguration viewModelType, Type viewType)
            {
                ViewModelType = viewModelType;
                ViewType = viewType;
            }
        }

        protected SplashScreenViewModel currentSplashScreen;
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
                Type viewType = viewFactory.GetViewType(splashScreenType.Type);
                splashScreens.Push(new SplashScreenContainer(splashScreenType, viewType));
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

            SplashScreenContainer splashScreenContainter = splashScreens.Pop();
            currentSplashScreen = viewModelFactory.GetViewModel(splashScreenContainter.ViewModelType.Type, splashScreenContainter.ViewModelType.Data) as SplashScreenViewModel;            
            currentSplashScreen.Closed += SplashScreen_Closed;

            ContentTemplate = dataTemplateFactory.GetDataTemplate(splashScreenContainter.ViewType);
            Content = currentSplashScreen;
        }

        protected virtual void ShowMainView()
        {
            if (ShellViewRequested != null)
                ShellViewRequested(this, EventArgs.Empty);
        }

        private void SplashScreen_Closed(object sender, SplashScreenEventArgs e)
        {
            currentSplashScreen.Closed -= SplashScreen_Closed;
                        
            if (!e.Result && ExitRequested != null)
                ExitRequested(sender, EventArgs.Empty);
            if (e.Result)
                ShowNextSplashScreen();
        }
    }*/
}
