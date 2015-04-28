using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace AppShell.Tests
{
    [Service("service1")]
    public interface IService1
    {
        [ServiceMethod("serviceMethod1")]
        void ServiceMethod1();
        [ServiceMethod("serviceMethod2")]
        void ServiceMethod2(int value);
    }

    public class Service1 : IService1
    {
        public bool ServiceMethod1Called { get; private set; }
        public int ServiceMethod2Result { get; private set; }

        public void ServiceMethod1()
        {
            ServiceMethod1Called = true;
        }

        public void ServiceMethod2(int value)
        {
            ServiceMethod2Result = value;
        }
    }
    
    public class ServiceDispatcherTest
    {
        private class TestPlatformProvider : IPlatformProvider
        {
            public IEnumerable<Assembly> GetAssemblies()
            {
                yield return typeof(TestPlatformProvider).GetTypeInfo().Assembly;
            }

            public void ExecuteOnUIThread(Action action)
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
        public void Services_WithoutInitialization_ShouldHaveNoServiceRegistered()
        {
            ServiceDispatcher serviceDispatcher = new ServiceDispatcher(new TestPlatformProvider());
            
            Assert.Equal(0, serviceDispatcher.Services.Count);
        }

        [Fact]
        public void Services_WithInitialization_ShouldHaveServiceRegistered()
        {
            ServiceDispatcher serviceDispatcher = new ServiceDispatcher(new TestPlatformProvider());
            serviceDispatcher.Initialize();

            Assert.Equal(1, serviceDispatcher.Services.Count);
            Assert.True(serviceDispatcher.Services.ContainsKey("service1"));
            Assert.Equal(2, serviceDispatcher.Services["service1"].Count());
            Assert.True(serviceDispatcher.Services["service1"].Contains("serviceMethod1"));
            Assert.True(serviceDispatcher.Services["service1"].Contains("serviceMethod2"));
        }

        [Fact]
        public void Dispatch_ServiceShouldHaveBeenCalled()
        {
            ServiceDispatcher serviceDispatcher = new ServiceDispatcher(new TestPlatformProvider());
            serviceDispatcher.Initialize();

            Service1 service1 = new Service1();

            serviceDispatcher.Subscribe<IService1>(service1);
            serviceDispatcher.Dispatch<IService1>(s => s.ServiceMethod1());

            Assert.True(service1.ServiceMethod1Called);
        }

        [Fact]
        public void DispatchReflection_ServiceShouldHaveBeenCalled()
        {
            ServiceDispatcher serviceDispatcher = new ServiceDispatcher(new TestPlatformProvider());
            serviceDispatcher.Initialize();

            Service1 service1 = new Service1();

            serviceDispatcher.Subscribe<IService1>(service1);
            serviceDispatcher.Dispatch("service1", "serviceMethod1", null);

            Assert.True(service1.ServiceMethod1Called);
        }

        [Fact]
        public void Dispatch_NoSubscribedService_ServiceShouldNotHaveBeenCalled()
        {
            ServiceDispatcher serviceDispatcher = new ServiceDispatcher(new TestPlatformProvider());
            serviceDispatcher.Initialize();

            Service1 service1 = new Service1();

            serviceDispatcher.Subscribe<IService1>(service1);
            serviceDispatcher.Unsubscribe<IService1>(service1);
            serviceDispatcher.Dispatch<IService1>(s => s.ServiceMethod1());

            Assert.False(service1.ServiceMethod1Called);
        }

        [Fact]
        public void DispatchReflection_NoSubscribedService_ServiceShouldNotHaveBeenCalled()
        {
            ServiceDispatcher serviceDispatcher = new ServiceDispatcher(new TestPlatformProvider());
            serviceDispatcher.Initialize();

            Service1 service1 = new Service1();

            serviceDispatcher.Subscribe<IService1>(service1);
            serviceDispatcher.Unsubscribe<IService1>(service1);
            serviceDispatcher.Dispatch("service1", "serviceMethod1", null);

            Assert.False(service1.ServiceMethod1Called);
        }

        [Fact]
        public void Dispatch_ServiceShouldHaveBeenCalledWithParameter()
        {
            ServiceDispatcher serviceDispatcher = new ServiceDispatcher(new TestPlatformProvider());
            serviceDispatcher.Initialize();

            Service1 service1 = new Service1();

            serviceDispatcher.Subscribe<IService1>(service1);
            serviceDispatcher.Dispatch<IService1>(s => s.ServiceMethod2(42));

            Assert.Equal(42, service1.ServiceMethod2Result);
        }

        [Fact]
        public void DispatchReflection_ServiceShouldHaveBeenCalledWithParameter()
        {
            ServiceDispatcher serviceDispatcher = new ServiceDispatcher(new TestPlatformProvider());
            serviceDispatcher.Initialize();

            Service1 service1 = new Service1();

            serviceDispatcher.Subscribe<IService1>(service1);
            serviceDispatcher.Dispatch("service1", "serviceMethod2", new object[] { 42 });

            Assert.Equal(42, service1.ServiceMethod2Result);
        }

        [Fact]
        public void DispatchReflection_ServiceShouldHaveBeenCalledWithDifferentParameterType()
        {
            ServiceDispatcher serviceDispatcher = new ServiceDispatcher(new TestPlatformProvider());
            serviceDispatcher.Initialize();

            Service1 service1 = new Service1();

            serviceDispatcher.Subscribe<IService1>(service1);
            serviceDispatcher.Dispatch("service1", "serviceMethod2", new object[] { 42L });

            Assert.Equal(42, service1.ServiceMethod2Result);
        }
    }
}
