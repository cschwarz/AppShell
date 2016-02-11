using System;
using System.Collections.Generic;

namespace AppShell
{
    public class LambdaEqualityComparer<T> : IEqualityComparer<T>
    {
        private Func<T, T, bool> equals;
        private Func<T, int> getHashCode;

        public LambdaEqualityComparer(Func<T, T, bool> equals)
            : this(equals, null)
        {
        }

        public LambdaEqualityComparer(Func<T, T, bool> equals, Func<T, int> getHashCode)
        {
            this.equals = equals;
            this.getHashCode = getHashCode;
        }

        public bool Equals(T x, T y)
        {
            return equals(x, y);
        }

        public int GetHashCode(T obj)
        {
            if (getHashCode != null)
                return getHashCode(obj);

            return new { obj }.GetHashCode();
        }
    }
}
