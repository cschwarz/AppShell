
using Xunit;

namespace AppShell.Mobile.Android.Tests
{
    public class AndroidPlatformProviderTest
    {
        [Fact]
        public void GetPlatform()
        {
            Assert.Equal(Platform.Android, new AndroidPlatformProvider().GetPlatform());
        }
    }
}