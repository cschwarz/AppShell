using Xunit;

namespace AppShell.Desktop.Tests
{
    public class DesktopPlatformProviderTest
    {
        [Fact]
        public void GetPlatform_ShouldReturnWindows()
        {
            Assert.Equal(Platform.Windows, new DesktopPlatformProvider().GetPlatform());
        }
    }
}
