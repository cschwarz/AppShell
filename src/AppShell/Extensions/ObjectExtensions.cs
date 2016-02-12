using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

            if (valueType.IsArray && conversionType.IsArray)
            {
                Type elementConversionType = conversionType.GetElementType();
                var valueArray = (value as Array);

                if (valueArray != null)
                {
                    Array array = Array.CreateInstance(elementConversionType, valueArray.Length);

                    for (int i = 0; i < valueArray.Length; i++)
                        array.SetValue(valueArray.GetValue(i).ChangeType(elementConversionType), i);

                    return array;
                }
            }
            else if (valueType.IsArray &&
                conversionType.GenericTypeArguments.Length == 1 &&
                conversionType.GetGenericTypeDefinition().MakeGenericType(typeof(object)).GetTypeInfo().IsAssignableFrom(typeof(List<object>).GetTypeInfo()))
            {
                Type genericConversionType = conversionType.GenericTypeArguments.Single();
                var valueArray = (value as Array);

                if (valueArray != null)
                {
                    IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(genericConversionType));

                    for (int i = 0; i < valueArray.Length; i++)
                        list.Add(valueArray.GetValue(i).ChangeType(genericConversionType));

                    return list;
                }
            }

            return Convert.ChangeType(value, conversionType);
        }

        public static Dictionary<string, object> ToDictionary(this object value)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            if (value == null)
                return dictionary;

            TypeInfo type = value.GetType().GetTypeInfo();
            foreach (PropertyInfo propertyInfo in type.DeclaredProperties)
                dictionary[propertyInfo.Name] = propertyInfo.GetValue(value);

            return dictionary;
        }
    }
}
