using System.Collections;
using System.Collections.Generic;

namespace AppShell
{
    public class TwoWayDictionary<T1, T2> : IEnumerable<KeyValuePair<T1, T2>>
    {
        public IDictionary<T1, T2> First { get; private set; }
        public IDictionary<T2, T1> Second { get; private set; }

        public TwoWayDictionary()
        {
            First = new Dictionary<T1, T2>();
            Second = new Dictionary<T2, T1>();
        }

        public TwoWayDictionary(IEqualityComparer<T1> comparer)
        {
            First = new Dictionary<T1, T2>(comparer);
            Second = new Dictionary<T2, T1>();
        }

        public TwoWayDictionary(IEqualityComparer<T2> comparer)
        {
            First = new Dictionary<T1, T2>();
            Second = new Dictionary<T2, T1>(comparer);
        }

        public TwoWayDictionary(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2)
        {
            First = new Dictionary<T1, T2>(comparer1);
            Second = new Dictionary<T2, T1>(comparer2);
        }

        public void Add(T1 key1, T2 key2)
        {
            First.Add(key1, key2);
            Second.Add(key2, key1);
        }

        public bool ContainsKey(T1 key)
        {
            return First.ContainsKey(key);
        }

        public bool ContainsKey(T2 key)
        {
            return Second.ContainsKey(key);
        }

        public void Remove(T1 key)
        {
            Second.Remove(First[key]);
            First.Remove(key);
        }

        public void Remove(T2 key)
        {
            First.Remove(Second[key]);
            Second.Remove(key);
        }

        public void Clear()
        {
            First.Clear();
            Second.Clear();
        }

        public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator()
        {
            return First.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return First.GetEnumerator();
        }

        public T2 this[T1 key] { get { return First[key]; } }
        public T1 this[T2 key] { get { return Second[key]; } }
    }
}