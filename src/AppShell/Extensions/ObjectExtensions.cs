using System.Reflection;

namespace System
{
    public static class ObjectExtensions
    {
        public static object ChangeType(this object value, Type conversionType)
        {
            if (value == null)
                return null;

            Type valueType = value.GetType();
                        
            TypeInfo valueTypeInfo = valueType.GetTypeInfo();
            TypeInfo conversionTypeInfo = conversionType.GetTypeInfo();

            if (conversionTypeInfo.IsAssignableFrom(valueTypeInfo))
                return value;

            return Convert.ChangeType(value, conversionType);
        }
    }
}
