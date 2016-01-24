using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AppShell.Tests
{
    public class PlatformTest
    {
        [Fact]
        public void HasFlag_Desktop()
        {
            Assert.True(Platform.Desktop.HasFlag(Platform.Windows));
            Assert.True(Platform.Desktop.HasFlag(Platform.Mac));

            Assert.False(Platform.Desktop.HasFlag(Platform.Android));
            Assert.False(Platform.Desktop.HasFlag(Platform.iOS));
            Assert.False(Platform.Desktop.HasFlag(Platform.WinRT));
            Assert.False(Platform.Desktop.HasFlag(Platform.UWP));
        }

        [Fact]
        public void HasFlag_Mobile()
        {
            Assert.True(Platform.Mobile.HasFlag(Platform.Android));
            Assert.True(Platform.Mobile.HasFlag(Platform.iOS));
            Assert.True(Platform.Mobile.HasFlag(Platform.WinRT));
            Assert.True(Platform.Mobile.HasFlag(Platform.UWP));

            Assert.False(Platform.Mobile.HasFlag(Platform.Windows));
            Assert.False(Platform.Mobile.HasFlag(Platform.Mac));
        }

        [Fact]
        public void HasFlag_All()
        {
            Assert.True(Platform.All.HasFlag(Platform.Android));
            Assert.True(Platform.All.HasFlag(Platform.iOS));
            Assert.True(Platform.All.HasFlag(Platform.WinRT));
            Assert.True(Platform.All.HasFlag(Platform.UWP));

            Assert.True(Platform.All.HasFlag(Platform.Windows));
            Assert.True(Platform.All.HasFlag(Platform.Mac));

            Assert.True(Platform.All.HasFlag(Platform.Desktop));
            Assert.True(Platform.All.HasFlag(Platform.Mobile));
        }
    }
}
