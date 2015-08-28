using System;
using System.Collections.Generic;
using Xunit;

namespace AppShell.Tests
{
    public class ViewModelResolutionTest
    {
        public class ViewModel1 : ViewModel
        {
        }

        public class ViewModel2
        {
        }

        public class ViewModel3 : ViewModel
        {
        }

        [ViewModel(Substitute = typeof(ViewModel3))]
        public class SubstituteViewModel3 : ViewModel3
        {
        }

        [Fact]
        public void ViewModel1InheritsViewModel_ShouldBeMapped()
        {
            IDictionary<Type, Type> viewModelMapping = new ViewModelResolution().GetViewModelMapping(new List<Type>() { typeof(ViewModel1) });
            Assert.Equal(1, viewModelMapping.Count);
            Assert.True(viewModelMapping.Contains(new KeyValuePair<Type, Type>(typeof(ViewModel1), typeof(ViewModel1))));
        }

        [Fact]
        public void ViewModel2DoesNotInheritViewModel_ShouldNotBeMapped()
        {
            IDictionary<Type, Type> viewModelMapping = new ViewModelResolution().GetViewModelMapping(new List<Type>() { typeof(ViewModel2) });
            Assert.Equal(0, viewModelMapping.Count);
        }

        [Fact]
        public void ViewModel3IsSubstituted_ShouldBeMapped()
        {
            IDictionary<Type, Type> viewModelMapping = new ViewModelResolution().GetViewModelMapping(new List<Type>() { typeof(ViewModel3), typeof(SubstituteViewModel3) });
            Assert.Equal(1, viewModelMapping.Count);
            Assert.True(viewModelMapping.Contains(new KeyValuePair<Type, Type>(typeof(ViewModel3), typeof(SubstituteViewModel3))));
        }

        [Fact]
        public void ViewModel3IsSubstituted_ReverseOrder_ShouldBeMapped()
        {
            IDictionary<Type, Type> viewModelMapping = new ViewModelResolution().GetViewModelMapping(new List<Type>() { typeof(SubstituteViewModel3), typeof(ViewModel3) });
            Assert.Equal(1, viewModelMapping.Count);
            Assert.True(viewModelMapping.Contains(new KeyValuePair<Type, Type>(typeof(ViewModel3), typeof(SubstituteViewModel3))));
        }
    }
}
