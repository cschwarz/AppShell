using System.IO;
using System.Reflection;

namespace AppShell
{
    public class WebBrowserViewModel : ViewModel
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
    }
}
