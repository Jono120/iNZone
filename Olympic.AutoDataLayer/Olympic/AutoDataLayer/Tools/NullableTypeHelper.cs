namespace Olympic.AutoDataLayer.Tools
{
    using System;

    internal class NullableTypeHelper
    {
        public static Type GetUnderlyingTypeFromNullableType(Type nullableType)
        {
            if (!IsNullableType(nullableType))
            {
                return nullableType;
            }
            return Nullable.GetUnderlyingType(nullableType);
        }

        public static bool IsNullableType(Type type)
        {
            return (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>)));
        }
    }
}

