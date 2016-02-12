using System;
using System.Collections.Generic;
using Xunit;

namespace AppShell.Tests
{
    public class ViewResolutionTest
    {
        public class ViewModel1
        {
        }

        public class SubViewModel1 : ViewModel1
        {
        }

        public class ViewModel2
        {
        }

        [View(typeof(ViewModel1))]
        public class View1
        {
        }

        public class View2
        {
        }

        [Fact]
        public void View1MappedToViewModel1_ShouldBeMapped()
        {
            IDictionary<Type, Type> viewMapping = new ViewResolution().GetViewMapping(new List<Type>() { typeof(ViewModel1), typeof(View1) });
            Assert.Equal(1, viewMapping.Count);
            Assert.True(viewMapping.Contains(new KeyValuePair<Type, Type>(typeof(ViewModel1), typeof(View1))));
        }

        [Fact]
        public void View1MappedToViewModel1_ReverseOrder_ShouldBeMapped()
        {
            IDictionary<Type, Type> viewMapping = new ViewResolution().GetViewMapping(new List<Type>() { typeof(View1), typeof(ViewModel1) });
            Assert.Equal(1, viewMapping.Count);
            Assert.True(viewMapping.Contains(new KeyValuePair<Type, Type>(typeof(ViewModel1), typeof(View1))));
        }

        [Fact]
        public void View2NotMappedToViewModel2_ShouldNotBeMapped()
        {
            IDictionary<Type, Type> viewMapping = new ViewResolution().GetViewMapping(new List<Type>() { typeof(ViewModel2), typeof(View2) });
            Assert.Equal(0, viewMapping.Count);
        }

        [Fact]
        public void View2NotMappedToViewModel2_ReverseOrder_ShouldNotBeMapped()
        {
            IDictionary<Type, Type> viewMapping = new ViewResolution().GetViewMapping(new List<Type>() { typeof(View2), typeof(ViewModel2) });
            Assert.Equal(0, viewMapping.Count);
        }

        [Fact]
        public void View1MappedToViewModel1WithDerivedSubViewModel1_ShouldReturnTwoMappings()
        {
            IDictionary<Type, Type> viewMapping = new ViewResolution().GetViewMapping(new List<Type>() { typeof(ViewModel1), typeof(SubViewModel1), typeof(View1) });
            Assert.Equal(2, viewMapping.Count);
            Assert.True(viewMapping.Contains(new KeyValuePair<Type, Type>(typeof(ViewModel1), typeof(View1))));
            Assert.True(viewMapping.Contains(new KeyValuePair<Type, Type>(typeof(SubViewModel1), typeof(View1))));
        }

        [Fact]
        public void View1MappedToViewModel1WithDerivedSubViewModel1_ReverseOrder_ShouldReturnTwoMappings()
        {
            IDictionary<Type, Type> viewMapping = new ViewResolution().GetViewMapping(new List<Type>() { typeof(View1), typeof(SubViewModel1), typeof(ViewModel1) });
            Assert.Equal(2, viewMapping.Count);
            Assert.True(viewMapping.Contains(new KeyValuePair<Type, Type>(typeof(ViewModel1), typeof(View1))));
            Assert.True(viewMapping.Contains(new KeyValuePair<Type, Type>(typeof(SubViewModel1), typeof(View1))));
        }
    }
}
