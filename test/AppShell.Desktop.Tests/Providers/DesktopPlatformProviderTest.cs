using Xunit;

namespace AppShell.Desktop.Tests
{
    public class DesktopPlatformProviderTest
    {
        [Fact]
        public void GetPlatform()
        {
            Assert.Equal(Platform.Windows, new DesktopPlatformProvider().GetPlatform());
        }
    }
}
