using System.IO;
using System.Reflection;

namespace AppShell
{
    public class WebBrowserViewModel : ViewModel, IWebBrowserService
    {
        private string url;
        public string Url
        {
            get { return url; }
            set
            {
                if (url != value)
                {
                    url = value;
                    OnPropertyChanged("Url");
                }
            }
        }

        private string script;
        public string Script
        {
            get { return script; }
            set
            {
                if (script != value)
                {
                    script = value;
                    OnPropertyChanged("Script");
                }
            }
        }

        private string html;
        public string Html
        {
            get { return html; }
            set
            {
                if (html != value)
                {
                    html = value;
                    OnPropertyChanged("Html");
                }
            }
        }

        private string embeddedHtml;
        public string EmbeddedHtml
        {
            get { return embeddedHtml; }
            set
            {
                if (embeddedHtml != value)
                {
                    embeddedHtml = value;

                    string[] manifestResourceParts = embeddedHtml.Split(';');

                    Assembly assembly = manifestResourceParts.Length > 1 ? Assembly.Load(new AssemblyName(manifestResourceParts[1])) : GetType().GetTypeInfo().Assembly;

                    using (StreamReader streamReader = new StreamReader(assembly.GetManifestResourceStream(manifestResourceParts[0])))
                        Html = streamReader.ReadToEnd();

                    OnPropertyChanged("EmbeddedHtml");
                }
            }
        }

        private IServiceDispatcher serviceDispatcher;

        public WebBrowserViewModel(IServiceDispatcher serviceDispatcher)
        {
            this.serviceDispatcher = serviceDispatcher;

            serviceDispatcher.Subscribe<IWebBrowserService>(this);
        }

        public override void Dispose()
        {
            base.Dispose();

            serviceDispatcher.Unsubscribe<IWebBrowserService>(this);
        }

        public void InvokeScript(string script)
        {
            Script = script;
        }

        public void Navigate(string url)
        {
            Url = url;
        }

        public void LoadHtml(string html)
        {
            Html = html;
        }

        public void LoadEmbeddedHtml(string embeddedHtml)
        {
            EmbeddedHtml = embeddedHtml;
        }
    }
}
