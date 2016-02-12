using System;
using System.Collections.Generic;
using System.Reflection;

namespace AppShell
{
    public class EventRegistration
    {
        public Guid Id { get; private set; }
        public object SubscribeAction { get; private set; }
        public object UnsubscribeAction { get; private set; }

        public EventRegistration(object subscribeAction, object unsubscribeAction)
        {
            Id = Guid.NewGuid();
            SubscribeAction = subscribeAction;
            UnsubscribeAction = unsubscribeAction;
        }
    }

    public interface IServiceDispatcher
    {
        IDictionary<string, IEnumerable<string>> Services { get; }

        void Initialize();

        void Subscribe<T>(T service) where T : IService;
        void Unsubscribe<T>(T service) where T : IService;

        void Dispatch<T>(Action<T> predicate) where T : IService;
        IEnumerable<TResult> Dispatch<T, TResult>(Func<T, TResult> predicate) where T : IService;
        IEnumerable<object> Dispatch(string serviceName, string methodName, object[] parameters);

        void Dispatch<T>(string instanceName, Action<T> predicate) where T : class, IService;
        TResult Dispatch<T, TResult>(string instanceName, Func<T, TResult> predicate) where T : class, IService;
        object Dispatch(string serviceName, string instanceName, string methodName, object[] parameters);

        EventRegistration SubscribeEvent<T>(Action<T> subscribe, Action<T> unsubscribe) where T : class;
        EventRegistration SubscribeEvent(string serviceName, string eventName, object target, Action<object, object> callback);
        void UnsubscribeEvent<T>(EventRegistration eventRegistration) where T : class;
        void UnsubscribeEvent(string serviceName, EventRegistration eventRegistration);
    }
}
