
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
    }
}
