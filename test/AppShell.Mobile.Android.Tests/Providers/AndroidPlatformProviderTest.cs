
using Xunit;

namespace AppShell.Mobile.Android.Tests
{
    public class AndroidPlatformProviderTest
    {
        [Fact]
        public void GetPlatform_ShouldReturnAndroid()
        {
            Assert.Equal(Platform.Android, new AndroidPlatformProvider().GetPlatform());
        }
    }
}