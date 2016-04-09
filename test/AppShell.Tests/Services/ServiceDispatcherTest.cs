using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace AppShell.Tests
{
    [Service("service1")]
    public interface IService1 : IService
    {
        event EventHandler Event1;
        event EventHandler<string> Event2;

        [ServiceMethod("serviceMethod1")]
        void ServiceMethod1();
        [ServiceMethod("serviceMethod2")]
        void ServiceMethod2(int value);
        [ServiceMethod("serviceMethod3")]
        int ServiceMethod3(int value1, int value2);
        [ServiceMethod("serviceMethod4")]
        void ServiceMethod4(string stringValue, long longValue, float floatValue, double doubleValue, string[] stringArray, long[] longArray, IEnumerable<string> stringEnumerable, IEnumerable<long> longEnumerable, IDictionary<string, string> stringDictionary);
    }

    public class Service1 : IService1
    {
        public string Name { get; private set; }
        public bool ServiceMethod1Called { get; private set; }
        public int ServiceMethod2Result { get; private set; }
        public DataTypeTest ServiceMethod4Result { get; private set; }

        public event EventHandler Event1;
        public event EventHandler<string> Event2;

        public Service1()
        {
        }

        public Service1(string name)
        {
            Name = name;
        }

        public void ServiceMethod1()
        {
            ServiceMethod1Called = true;
        }

        public void ServiceMethod2(int value)
        {
            ServiceMethod2Result = value;
        }

        public int ServiceMethod3(int value1, int value2)
        {
            return value1 + value2;
        }

        public void ServiceMethod4(string stringValue, long longValue, float floatValue, double doubleValue, string[] stringArray, long[] longArray, IEnumerable<string> stringEnumerable, IEnumerable<long> longEnumerable, IDictionary<string, string> stringDictionary)
        {
            ServiceMethod4Result = new DataTypeTest(stringValue, longValue, floatValue, doubleValue, stringArray, longArray, stringEnumerable, longEnumerable, stringDictionary);
        }

        public void RaiseEvent1()
        {
            if (Event1 != null)
                Event1(this, EventArgs.Empty);
        }

        public void RaiseEvent2(string value)
        {
            if (Event2 != null)
                Event2(this, value);
        }
    }

    [Service("service2")]
    public interface IService2 : IService
    {
    }

    public class Service2 : IService2
    {
        public string Name { get { return "Service2"; } }
    }

    public class DataTypeTest : IEquatable<DataTypeTest>
    {
        public string StringValue { get; private set; }
        public long LongValue { get; private set; }
        public float FloatValue { get; private set; }
        public double DoubleValue { get; private set; }
        public string[] StringArray { get; private set; }
        public long[] LongArray { get; private set; }
        public IEnumerable<string> StringEnumerable { get; private set; }
        public IEnumerable<long> LongEnumerable { get; private set; }
        public IDictionary<string, string> StringDictionary { get; private set; }

        public DataTypeTest(string stringValue, long longValue, float floatValue, double doubleValue, string[] stringArray, long[] longArray, IEnumerable<string> stringEnumerable, IEnumerable<long> longEnumerable, IDictionary<string, string> stringDictionary)
        {
            StringValue = stringValue;
            LongValue = longValue;
            FloatValue = floatValue;
            DoubleValue = doubleValue;
            StringArray = stringArray;
            LongArray = longArray;
            StringEnumerable = stringEnumerable;
            LongEnumerable = longEnumerable;
            StringDictionary = stringDictionary;
        }

        public bool Equals(DataTypeTest other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return StringValue == other.StringValue &&
                LongValue == other.LongValue &&
                FloatValue == other.FloatValue &&
                DoubleValue == other.DoubleValue &&
                StringArray == other.StringArray &&
                LongArray == other.LongArray &&
                StringEnumerable == other.StringEnumerable &&
                LongEnumerable == other.LongEnumerable &&
                StringDictionary == other.StringDictionary;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((DataTypeTest)obj);
        }

        public override int GetHashCode()
        {
            return new { StringValue, LongValue, FloatValue, DoubleValue, StringArray, LongArray, StringEnumerable, LongEnumerable, StringDictionary }.GetHashCode();
        }
    }

    public class ServiceDispatcherTest
    {
        private class TestPlatformProvider : IPlatformProvider
        {
            public Platform GetPlatform()
            {
                return Platform.None;
            }

            public IEnumerable<Assembly> GetAssemblies()
            {
                yield return typeof(TestPlatformProvider).GetTypeInfo().Assembly;
            }

            public void ExecuteOnUIThread(Action action)
            {
                throw new NotImplementedException();
            }

            public void ShowMessage(string title, string message, string cancel)
            {
                throw new NotImplementedException();
            }

            public string GetDocumentFolderPath()
            {
                throw new NotImplementedException();
            }

            public void OpenUrl(string url)
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

            Assert.Equal(2, serviceDispatcher.Services.Count);
            Assert.True(serviceDispatcher.Services.ContainsKey("service1"));
            Assert.True(serviceDispatcher.Services.ContainsKey("service2"));
            Assert.Equal(4, serviceDispatcher.Services["service1"].Count());
            Assert.True(serviceDispatcher.Services["service1"].Contains("serviceMethod1"));
            Assert.True(serviceDispatcher.Services["service1"].Contains("serviceMethod2"));
            Assert.True(serviceDispatcher.Services["service1"].Contains("serviceMethod3"));
            Assert.True(serviceDispatcher.Services["service1"].Contains("serviceMethod4"));
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
        public void Dispatch_NamedService_ServiceShouldHaveBeenCalled()
        {
            ServiceDispatcher serviceDispatcher = new ServiceDispatcher(new TestPlatformProvider());
            serviceDispatcher.Initialize();

            Service1 instance1 = new Service1("Instance1");
            Service1 instance2 = new Service1("Instance2");

            serviceDispatcher.Subscribe<IService1>(instance1);
            serviceDispatcher.Subscribe<IService1>(instance2);
            serviceDispatcher.Dispatch<IService1>("Instance1", s => s.ServiceMethod1());

            Assert.True(instance1.ServiceMethod1Called);
            Assert.False(instance2.ServiceMethod1Called);
        }

        [Fact]
        public void DispatchReflection_NamedService_ServiceShouldHaveBeenCalled()
        {
            ServiceDispatcher serviceDispatcher = new ServiceDispatcher(new TestPlatformProvider());
            serviceDispatcher.Initialize();

            Service1 instance1 = new Service1("Instance1");
            Service1 instance2 = new Service1("Instance2");

            serviceDispatcher.Subscribe<IService1>(instance1);
            serviceDispatcher.Subscribe<IService1>(instance2);
            serviceDispatcher.Dispatch("service1", "Instance1", "serviceMethod1", null);

            Assert.True(instance1.ServiceMethod1Called);
            Assert.False(instance2.ServiceMethod1Called);
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

        [Fact]
        public void Dispatch_ServiceShouldReturnValue()
        {
            ServiceDispatcher serviceDispatcher = new ServiceDispatcher(new TestPlatformProvider());
            serviceDispatcher.Initialize();

            Service1 service1 = new Service1();

            serviceDispatcher.Subscribe<IService1>(service1);
            Assert.Equal(3, serviceDispatcher.Dispatch<IService1, int>(s => s.ServiceMethod3(1, 2)).Single());
        }

        [Fact]
        public void DispatchReflection_ServiceShouldReturnValue()
        {
            ServiceDispatcher serviceDispatcher = new ServiceDispatcher(new TestPlatformProvider());
            serviceDispatcher.Initialize();

            Service1 service1 = new Service1();

            serviceDispatcher.Subscribe<IService1>(service1);
            Assert.Equal(3, serviceDispatcher.Dispatch("service1", "serviceMethod3", new object[] { 1, 2 }).Cast<int>().Single());
        }

        [Fact]
        public void Dispatch_NamedService_ServiceShouldReturnValue()
        {
            ServiceDispatcher serviceDispatcher = new ServiceDispatcher(new TestPlatformProvider());
            serviceDispatcher.Initialize();

            Service1 instance1 = new Service1("Instance1");
            Service1 instance2 = new Service1("Instance2");

            serviceDispatcher.Subscribe<IService1>(instance1);
            serviceDispatcher.Subscribe<IService1>(instance2);
            Assert.Equal(3, serviceDispatcher.Dispatch<IService1, int>("Instance1", s => s.ServiceMethod3(1, 2)));
        }

        [Fact]
        public void DispatchReflection_NamedService_ServiceShouldReturnValue()
        {
            ServiceDispatcher serviceDispatcher = new ServiceDispatcher(new TestPlatformProvider());
            serviceDispatcher.Initialize();

            Service1 instance1 = new Service1("Instance1");
            Service1 instance2 = new Service1("Instance2");

            serviceDispatcher.Subscribe<IService1>(instance1);
            serviceDispatcher.Subscribe<IService1>(instance2);
            Assert.Equal(3, Convert.ToInt32(serviceDispatcher.Dispatch("service1", "Instance1", "serviceMethod3", new object[] { 1, 2 })));
        }

        [Fact]
        public void Dispatch_ServiceShouldBeCalledWithVariousDataTypes()
        {
            DataTypeTest dataTypeTest = new DataTypeTest("StringValue", 1L, 2.2f, 3.3d, new string[] { "1", "2", "3" }, new long[] { 1, 2, 3 }, new List<string>() { "1", "2", "3" }, new List<long>() { 1, 2, 3 }, new Dictionary<string, string>() { { "Key1", "Value1" }, { "Key2", "Value2" } });

            ServiceDispatcher serviceDispatcher = new ServiceDispatcher(new TestPlatformProvider());
            serviceDispatcher.Initialize();

            Service1 service1 = new Service1();

            serviceDispatcher.Subscribe<IService1>(service1);
            serviceDispatcher.Dispatch<IService1>(s => s.ServiceMethod4(dataTypeTest.StringValue, dataTypeTest.LongValue, dataTypeTest.FloatValue, dataTypeTest.DoubleValue, dataTypeTest.StringArray, dataTypeTest.LongArray, dataTypeTest.StringEnumerable, dataTypeTest.LongEnumerable, dataTypeTest.StringDictionary));
            Assert.Equal(dataTypeTest, service1.ServiceMethod4Result);
        }

        [Fact]
        public void DispatchReflection_ServiceShouldBeCalledWithVariousDataTypes()
        {
            DataTypeTest dataTypeTest = new DataTypeTest("StringValue", 1L, 2.2f, 3.3d, new string[] { "1", "2", "3" }, new long[] { 1, 2, 3 }, new List<string>() { "1", "2", "3" }, new List<long>() { 1, 2, 3 }, new Dictionary<string, string>() { { "Key1", "Value1" }, { "Key2", "Value2" } });

            ServiceDispatcher serviceDispatcher = new ServiceDispatcher(new TestPlatformProvider());
            serviceDispatcher.Initialize();

            Service1 service1 = new Service1();

            serviceDispatcher.Subscribe<IService1>(service1);
            serviceDispatcher.Dispatch("service1", "serviceMethod4", new object[] { dataTypeTest.StringValue, dataTypeTest.LongValue, dataTypeTest.FloatValue, dataTypeTest.DoubleValue, dataTypeTest.StringArray, dataTypeTest.LongArray, dataTypeTest.StringEnumerable, dataTypeTest.LongEnumerable, dataTypeTest.StringDictionary });
            Assert.Equal(dataTypeTest, service1.ServiceMethod4Result);
        }

        [Fact]
        public void EventShouldBeRaised()
        {
            bool event1Raised = false;
            EventHandler event1Handler = (o, e) => { event1Raised = true; };

            IServiceDispatcher serviceDispatcher = new ServiceDispatcher(new TestPlatformProvider());
            serviceDispatcher.Initialize();

            Service1 service1 = new Service1();

            serviceDispatcher.Subscribe<IService1>(service1);
            serviceDispatcher.SubscribeEvent<IService1>(s => s.Event1 += event1Handler, s => s.Event1 -= event1Handler);

            service1.RaiseEvent1();

            Assert.True(event1Raised);
        }

        [Fact]
        public void EventShouldBeRaisedWithParameter()
        {
            string event2Raised = string.Empty;
            EventHandler<string> event2Handler = (o, e) => { event2Raised = e; };

            IServiceDispatcher serviceDispatcher = new ServiceDispatcher(new TestPlatformProvider());
            serviceDispatcher.Initialize();

            Service1 service1 = new Service1();

            serviceDispatcher.Subscribe<IService1>(service1);
            serviceDispatcher.SubscribeEvent<IService1>(s => s.Event2 += event2Handler, s => s.Event2 -= event2Handler);

            service1.RaiseEvent2("Event2");

            Assert.Equal("Event2", event2Raised);
        }

        [Fact]
        public void EventShouldNotBeRaisedAfterEventUnsubscribe()
        {
            bool event1Raised = false;
            EventHandler event1Handler = (o, e) => { event1Raised = true; };

            IServiceDispatcher serviceDispatcher = new ServiceDispatcher(new TestPlatformProvider());
            serviceDispatcher.Initialize();

            Service1 service1 = new Service1();

            serviceDispatcher.Subscribe<IService1>(service1);
            EventRegistration eventRegistration = serviceDispatcher.SubscribeEvent<IService1>(s => s.Event1 += event1Handler, s => s.Event1 -= event1Handler);
            serviceDispatcher.UnsubscribeEvent<IService1>(eventRegistration);

            service1.RaiseEvent1();

            Assert.False(event1Raised);
        }

        [Fact]
        public void EventShouldNotBeRaisedAfterServiceUnsubscribe()
        {
            bool event1Raised = false;
            EventHandler event1Handler = (o, e) => { event1Raised = true; };

            IServiceDispatcher serviceDispatcher = new ServiceDispatcher(new TestPlatformProvider());
            serviceDispatcher.Initialize();

            Service1 service1 = new Service1();

            serviceDispatcher.Subscribe<IService1>(service1);
            EventRegistration eventRegistration = serviceDispatcher.SubscribeEvent<IService1>(s => s.Event1 += event1Handler, s => s.Event1 -= event1Handler);
            serviceDispatcher.Unsubscribe<IService1>(service1);

            service1.RaiseEvent1();

            Assert.False(event1Raised);
        }

        [Fact]
        public void ShouldSuccessfullySubscribeWithMultipleSubscribedServices()
        {
            EventHandler event1Handler = (o, e) => { };

            IServiceDispatcher serviceDispatcher = new ServiceDispatcher(new TestPlatformProvider());
            serviceDispatcher.Initialize();

            Service1 service1 = new Service1();
            Service2 service2 = new Service2();

            serviceDispatcher.Subscribe<IService1>(service1);
            serviceDispatcher.SubscribeEvent<IService1>(s => s.Event1 += event1Handler, s => s.Event1 -= event1Handler);
            serviceDispatcher.Subscribe<IService2>(service2);
        }

        [Fact]
        public void ShouldSuccessfullyUnsubscribeWithMultipleSubscribedServices()
        {
            EventHandler event1Handler = (o, e) => { };

            IServiceDispatcher serviceDispatcher = new ServiceDispatcher(new TestPlatformProvider());
            serviceDispatcher.Initialize();

            Service1 service1 = new Service1();
            Service2 service2 = new Service2();

            serviceDispatcher.Subscribe<IService1>(service1);
            serviceDispatcher.Subscribe<IService2>(service2);
            serviceDispatcher.SubscribeEvent<IService1>(s => s.Event1 += event1Handler, s => s.Event1 -= event1Handler);
            serviceDispatcher.Unsubscribe<IService2>(service2);
        }
    }
}
