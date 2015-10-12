using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AppShell
{
    public class ServiceDispatcher : IServiceDispatcher
    {
        private static MethodInfo createEventHandlerMethod;

        private Dictionary<Type, List<IService>> subscribedServices;

        private Dictionary<string, IEnumerable<string>> services;
        private Dictionary<string, Type> serviceNameTypeMapping;
        private Dictionary<string, Dictionary<string, MethodInfo>> serviceMethodMapping;

        private Dictionary<Type, List<EventRegistration>> eventRegistrations;

        private IPlatformProvider platformProvider;

        public IDictionary<string, IEnumerable<string>> Services { get { return services; } }

        static ServiceDispatcher()
        {
            createEventHandlerMethod = typeof(ServiceDispatcher).GetRuntimeMethods().Single(m => m.Name == "CreateEventHandler");
        }

        public ServiceDispatcher(IPlatformProvider platformProvider)
        {
            this.platformProvider = platformProvider;

            subscribedServices = new Dictionary<Type, List<IService>>();

            services = new Dictionary<string, IEnumerable<string>>();
            serviceNameTypeMapping = new Dictionary<string, Type>();
            serviceMethodMapping = new Dictionary<string, Dictionary<string, MethodInfo>>();

            eventRegistrations = new Dictionary<Type, List<EventRegistration>>();
        }

        public void Initialize()
        {
            foreach (Assembly assembly in platformProvider.GetAssemblies<ShellResourceAttribute>())
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

        public void Subscribe<T>(T service) where T : IService
        {
            Type serviceType = typeof(T);

            if (!subscribedServices.ContainsKey(serviceType))
                subscribedServices.Add(serviceType, new List<IService>());

            subscribedServices[serviceType].Add(service);

            foreach (var eventRegistration in eventRegistrations)
            {
                if (subscribedServices.ContainsKey(eventRegistration.Key))
                {
                    foreach (var subscribeAction in eventRegistration.Value.Select(a => a.SubscribeAction).Cast<Action<T>>())
                        subscribeAction(service);
                }
            }
        }

        public void Unsubscribe<T>(T service) where T : IService
        {
            Type serviceType = typeof(T);

            if (!subscribedServices.ContainsKey(serviceType))
                return;

            subscribedServices[serviceType].Remove(service);

            foreach (var eventRegistration in eventRegistrations)
            {
                if (subscribedServices.ContainsKey(eventRegistration.Key))
                {
                    foreach (var unsubscribeAction in eventRegistration.Value.Select(a => a.UnsubscribeAction).Cast<Action<T>>())
                        unsubscribeAction(service);
                }
            }
        }

        public void Dispatch<T>(Action<T> predicate) where T : IService
        {
            Type serviceType = typeof(T);

            if (subscribedServices.ContainsKey(serviceType))
            {
                IEnumerable<T> services = subscribedServices[serviceType].Cast<T>().ToList();

                foreach (T service in services)
                    predicate(service);
            }
        }
                
        public IEnumerable<TResult> Dispatch<T, TResult>(Func<T, TResult> predicate) where T : IService
        {
            List<TResult> results = new List<TResult>();

            Type serviceType = typeof(T);

            if (subscribedServices.ContainsKey(serviceType))
            {
                IEnumerable<T> services = subscribedServices[serviceType].Cast<T>().ToList();

                foreach (T service in services)
                    results.Add(predicate(service));
            }

            return results;
        }

        public IEnumerable<object> Dispatch(string serviceName, string methodName, object[] parameters)
        {
            List<object> results = new List<object>();

            if (!serviceNameTypeMapping.ContainsKey(serviceName))
                return results;

            Type serviceType = serviceNameTypeMapping[serviceName];

            if (subscribedServices.ContainsKey(serviceType))
            {
                IEnumerable<object> services = subscribedServices[serviceType].ToList();

                MethodInfo method = serviceMethodMapping[serviceName][methodName];

                if (parameters != null)
                {
                    ParameterInfo[] parameterInfos = method.GetParameters();
                    parameters = parameters.Select((p, i) => p.ChangeType(parameterInfos[i].ParameterType)).ToArray();
                }

                foreach (object service in services)
                    results.Add(method.Invoke(service, parameters));
            }

            return results;
        }

        public void Dispatch<T>(string instanceName, Action<T> predicate) where T : class, IService
        {
            Type serviceType = typeof(T);

            if (subscribedServices.ContainsKey(serviceType))
            {
                T service = subscribedServices[serviceType].Single(s => s.Name == instanceName) as T;
                predicate(service);
            }
        }

        public TResult Dispatch<T, TResult>(string instanceName, Func<T, TResult> predicate) where T : class, IService
        {
            Type serviceType = typeof(T);

            if (subscribedServices.ContainsKey(serviceType))
            {
                T service = subscribedServices[serviceType].Single(s => s.Name == instanceName) as T;
                return predicate(service);
            }
            
            return default(TResult);
        }

        public object Dispatch(string serviceName, string instanceName, string methodName, object[] parameters)
        {
            if (!serviceNameTypeMapping.ContainsKey(serviceName))
                return null;

            Type serviceType = serviceNameTypeMapping[serviceName];

            if (subscribedServices.ContainsKey(serviceType))
            {
                IService service = subscribedServices[serviceType].Single(s => s.Name == instanceName);

                MethodInfo method = serviceMethodMapping[serviceName][methodName];

                if (parameters != null)
                {
                    ParameterInfo[] parameterInfos = method.GetParameters();
                    parameters = parameters.Select((p, i) => p.ChangeType(parameterInfos[i].ParameterType)).ToArray();
                }

                return method.Invoke(service, parameters);                
            }

            return null;
        }

        public EventRegistration SubscribeEvent<T>(Action<T> subscribe, Action<T> unsubscribe) where T : class
        {
            if (!eventRegistrations.ContainsKey(typeof(T)))
                eventRegistrations.Add(typeof(T), new List<EventRegistration>());

            EventRegistration eventRegistration = new EventRegistration(subscribe, unsubscribe);

            eventRegistrations[typeof(T)].Add(eventRegistration);

            if (subscribedServices.ContainsKey(typeof(T)))
            {
                foreach (var service in subscribedServices[typeof(T)])
                    subscribe(service as T);
            }

            return eventRegistration;
        }
        
        public EventRegistration SubscribeEvent(string serviceName, string eventName, object target, Action<object, object> callback)
        {
            if (!serviceNameTypeMapping.ContainsKey(serviceName))
                throw new Exception();

            Type serviceType = serviceNameTypeMapping[serviceName];

            if (!eventRegistrations.ContainsKey(serviceType))
                eventRegistrations.Add(serviceType, new List<EventRegistration>());
            
            EventInfo eventInfo = serviceType.GetRuntimeEvent(eventName);
            Type eventType = eventInfo.EventHandlerType.GetRuntimeMethods().Single(m => m.Name == "Invoke").GetParameters()[1].ParameterType;
            
            MulticastDelegate handler = (MulticastDelegate)createEventHandlerMethod.MakeGenericMethod(eventType).Invoke(this, new object[] { callback });            

            Action<object> subscribe = s => { eventInfo.AddEventHandler(s, handler); };
            Action<object> unsubscribe = s => { eventInfo.RemoveEventHandler(s, handler); };

            EventRegistration eventRegistration = new EventRegistration(subscribe, unsubscribe);

            eventRegistrations[serviceType].Add(eventRegistration);

            if (subscribedServices.ContainsKey(serviceType))
            {
                foreach (var service in subscribedServices[serviceType])
                    eventInfo.AddEventHandler(service, handler);
            }

            return eventRegistration;
        }

        public void UnsubscribeEvent<T>(EventRegistration eventRegistration) where T : class
        {
            if (eventRegistrations.ContainsKey(typeof(T)))
                eventRegistrations[typeof(T)].Remove(eventRegistration);

            if (subscribedServices.ContainsKey(typeof(T)))
            {
                foreach (var service in subscribedServices[typeof(T)])
                    ((Action<T>)eventRegistration.UnsubscribeAction)(service as T);
            }
        }

        public void UnsubscribeEvent(string serviceName, EventRegistration eventRegistration)
        {
            if (!serviceNameTypeMapping.ContainsKey(serviceName))
                throw new Exception();

            Type serviceType = serviceNameTypeMapping[serviceName];

            if (eventRegistrations.ContainsKey(serviceType))
                eventRegistrations[serviceType].Remove(eventRegistration);

            if (subscribedServices.ContainsKey(serviceType))
            {
                foreach (var service in subscribedServices[serviceType])
                    ((Action<object>)eventRegistration.UnsubscribeAction)(service);
            }
        }

        private static EventHandler<T> CreateEventHandler<T>(Action<object, object> callback) where T : class
        {
            return new EventHandler<T>(callback);
        }
    }
}