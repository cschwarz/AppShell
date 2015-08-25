using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppShell.Samples.ServiceDispatcher
{
    public class ManifestResourceWebBrowserViewModel : WebBrowserViewModel
    {
        private string manifestResource;
        public string ManifestResource
        {
            get { return manifestResource; }
            set
            {
                if (manifestResource != value)
                {
                    manifestResource = value;

                    using (StreamReader streamReader = new StreamReader(GetType().GetTypeInfo().Assembly.GetManifestResourceStream(manifestResource)))
                        Html = streamReader.ReadToEnd();

                    OnPropertyChanged("ManifestResource");
                }
            }
        }
    }
}
