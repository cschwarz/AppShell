
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
    }
}
