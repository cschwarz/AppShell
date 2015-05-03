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

        void Subscribe<T>(T service);
        void Unsubscribe<T>(T service);

        void Dispatch<T>(Action<T> predicate);
        IEnumerable<TResult> Dispatch<T, TResult>(Func<T, TResult> predicate);
        IEnumerable<object> Dispatch(string serviceName, string methodName, object[] parameters);

        EventRegistration SubscribeEvent<T>(Action<T> subscribe, Action<T> unsubscribe) where T : class;
        EventRegistration SubscribeEvent(string serviceName, string eventName, object target, Action<object, object> callback);
        void UnsubscribeEvent<T>(EventRegistration eventRegistration) where T : class;
        void UnsubscribeEvent(string serviceName, EventRegistration eventRegistration);
    }
}
