using System;
using System.Collections.Generic;
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

        [Fact]
        public void ChangeType_ObjectArrayOfStringsToStringEnumerable()
        {
            Assert.Equal(new List<string>() { "1", "2", "3" }, new object[] { "1", "2", "3" }.ChangeType(typeof(IEnumerable<string>)));
        }

        [Fact]
        public void ChangeType_ObjectArrayOfIntsToStringEnumerable()
        {
            Assert.Equal(new List<string>() { "1", "2", "3" }, new object[] { 1, 2, 3 }.ChangeType(typeof(IEnumerable<string>)));
        }

        [Fact]
        public void ChangeType_ObjectArrayOfStringsToStringList()
        {
            Assert.Equal(new List<string>() { "1", "2", "3" }, new object[] { "1", "2", "3" }.ChangeType(typeof(List<string>)));
        }

        [Fact]
        public void ChangeType_ObjectArrayOfIntsToStringList()
        {
            Assert.Equal(new List<string>() { "1", "2", "3" }, new object[] { 1, 2, 3 }.ChangeType(typeof(List<string>)));
        }

        [Fact(Skip = "NotImplemented")]
        public void ChangeType_ObjectArrayOfStringsToStringHashSet()
        {
            Assert.Equal(new HashSet<string>() { "1", "2", "3" }, new object[] { "1", "2", "3" }.ChangeType(typeof(HashSet<string>)));
        }

        [Fact(Skip = "NotImplemented")]
        public void ChangeType_ObjectArrayOfIntsToStringHashSet()
        {
            Assert.Equal(new HashSet<string>() { "1", "2", "3" }, new object[] { 1, 2, 3 }.ChangeType(typeof(HashSet<string>)));
        }
    }
}