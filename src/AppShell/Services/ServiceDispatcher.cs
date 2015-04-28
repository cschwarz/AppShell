using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AppShell
{
    public class ServiceDispatcher : IServiceDispatcher
    {
        private Dictionary<Type, List<object>> subscribedServices;

        private Dictionary<string, IEnumerable<string>> services;
        private Dictionary<string, Type> serviceNameTypeMapping;
        private Dictionary<string, Dictionary<string, MethodInfo>> serviceMethodMapping;

        private IPlatformProvider platformProvider;

        public IDictionary<string, IEnumerable<string>> Services { get { return services; } }

        public ServiceDispatcher(IPlatformProvider platformProvider)
        {
            this.platformProvider = platformProvider;

            subscribedServices = new Dictionary<Type, List<object>>();

            services = new Dictionary<string, IEnumerable<string>>();
            serviceNameTypeMapping = new Dictionary<string, Type>();
            serviceMethodMapping = new Dictionary<string, Dictionary<string, MethodInfo>>();
        }

        public void Initialize()
        {
            foreach (Assembly assembly in platformProvider.GetAssemblies<AppShellResourceAttribute>())
            {
                foreach (Type type in assembly.ExportedTypes)
                {
                    ServiceAttribute serviceAttribute = type.GetTypeInfo().GetCustomAttribute<ServiceAttribute>();

                    if (serviceAttribute == null)
                        continue;

                    List<string> serviceMethods = new List<string>();

                    serviceMethodMapping[serviceAttribute.ServiceName] = new Dictionary<string, MethodInfo>();

                    foreach (MethodInfo methodInfo in type.GetRuntimeMethods())
                    {
                        ServiceMethodAttribute serviceMethodAttribute = methodInfo.GetCustomAttribute<ServiceMethodAttribute>();

                        if (serviceMethodAttribute == null)
                            continue;

                        serviceMethods.Add(serviceMethodAttribute.MethodName);
                        serviceMethodMapping[serviceAttribute.ServiceName].Add(serviceMethodAttribute.MethodName, methodInfo);
                    }

                    services.Add(serviceAttribute.ServiceName, serviceMethods);
                    serviceNameTypeMapping.Add(serviceAttribute.ServiceName, type);
                }
            }
        }

        public void Subscribe<T>(T service)
        {
            Type serviceType = typeof(T);

            if (!subscribedServices.ContainsKey(serviceType))
                subscribedServices.Add(serviceType, new List<object>());

            subscribedServices[serviceType].Add(service);
        }

        public void Unsubscribe<T>(T service)
        {
            Type serviceType = typeof(T);

            if (!subscribedServices.ContainsKey(serviceType))
                return;

            subscribedServices[serviceType].Remove(service);
        }

        public void Dispatch<T>(Action<T> predicate)
        {
            Type serviceType = typeof(T);

            if (subscribedServices.ContainsKey(serviceType))
            {
                IEnumerable<T> services = subscribedServices[serviceType].Cast<T>();

                foreach (T service in services)
                    predicate(service);
            }
        }

        public void Dispatch(string serviceName, string methodName, object[] parameters)
        {
            if (!serviceNameTypeMapping.ContainsKey(serviceName))
                return;

            Type serviceType = serviceNameTypeMapping[serviceName];

            if (subscribedServices.ContainsKey(serviceType))
            {
                IEnumerable<object> services = subscribedServices[serviceType];

                MethodInfo method = serviceMethodMapping[serviceName][methodName];

                if (parameters != null)
                {
                    ParameterInfo[] parameterInfos = method.GetParameters();
                    parameters = parameters.Select((p, i) => Convert.ChangeType(p, parameterInfos[i].ParameterType)).ToArray();
                }

                foreach (object service in services)
                    method.Invoke(service, parameters);
            }
        }
    }
}