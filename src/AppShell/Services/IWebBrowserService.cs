namespace AppShell
{
    public interface IWebBrowserService : IService
    {
        void Navigate(string url);
        void LoadHtml(string html);
        void LoadEmbeddedHtml(string embeddedHtml);
    }
}
