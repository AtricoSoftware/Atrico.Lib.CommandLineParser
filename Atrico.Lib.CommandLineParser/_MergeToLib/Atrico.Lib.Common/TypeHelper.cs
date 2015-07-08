using System;

// ReSharper disable once CheckNamespace

namespace Atrico.Lib.Common.Reflection
{
    public static class TypeHelper
    {
        public static bool IsNullable(this Type type)
        {
            Type dummy;
            return IsNullable(type, out dummy);
        }

        public static bool IsNullable(this Type type, out Type underlyingType)
        {
            if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof (Nullable<>))
            {
                underlyingType = null;
                return false;
            }
            underlyingType = type.GetGenericArguments()[0];
            return true;
        }

        public static bool IsEnum(this Type type)
        {
            return type.BaseType == typeof (Enum);
        }
    }
}
