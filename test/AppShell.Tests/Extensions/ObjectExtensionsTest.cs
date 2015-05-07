using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AppShell.Tests
{
    public class ObjectExtensionsTest
    {
        [Fact]
        public void ChangeType_StringToString()
        {
            Assert.Equal("1", "1".ChangeType(typeof(string)));
        }

        [Fact]
        public void ChangeType_IntToInt()
        {
            Assert.Equal(1, 1.ChangeType(typeof(int)));
        }

        [Fact]
        public void ChangeType_IntToString()
        {
            Assert.Equal("1", 1.ChangeType(typeof(string)));
        }

        [Fact]
        public void ChangeType_StringToInt()
        {
            Assert.Equal(1, "1".ChangeType(typeof(int)));
        }

        [Fact]
        public void ChangeType_ObjectArrayOfStringsToStringArray()
        {
            Assert.Equal(new string[] { "1", "2", "3" }, new object[] { "1", "2", "3" }.ChangeType(typeof(string[])));
        }

        [Fact]
        public void ChangeType_ObjectArrayOfIntsToStringArray()
        {
            Assert.Equal(new string[] { "1", "2", "3" }, new object[] { 1, 2, 3 }.ChangeType(typeof(string[])));
        }

        [Fact(Skip = "NotImplemented")]
        public void ChangeType_ObjectArrayOfStringsToStringEnumerable()
        {
            Assert.Equal(new List<string>() { "1", "2", "3" }, new object[] { "1", "2", "3" }.ChangeType(typeof(IEnumerable<string>)));
        }

        [Fact(Skip = "NotImplemented")]
        public void ChangeType_ObjectArrayOfIntsToStringEnumerable()
        {
            Assert.Equal(new List<string>() { "1", "2", "3" }, new object[] { 1, 2, 3 }.ChangeType(typeof(IEnumerable<string>)));
        }
    }
}