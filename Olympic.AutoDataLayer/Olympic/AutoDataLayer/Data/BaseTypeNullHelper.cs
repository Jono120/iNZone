namespace Olympic.AutoDataLayer.Data
{
    using System;

    internal class BaseTypeNullHelper
    {
        private static DateTime _nullDate = DateTime.MinValue;
        private static double _nullDouble = double.MinValue;
        private static long _nullEnum = -1L;
        private static int _nullInt = -2147483648;

        private static bool IsNull(DateTime value)
        {
            return (value == _nullDate);
        }

        private static bool IsNull(double value)
        {
            return (value == _nullDouble);
        }

        private static bool IsNull(Enum value)
        {
            return (Convert.ToInt32(value) == _nullEnum);
        }

        private static bool IsNull(int value)
        {
            return (value == _nullInt);
        }

        public static bool IsNull(object value)
        {
            if (value == null)
            {
                return true;
            }
            Type type = value.GetType();
            if (type.IsEnum)
            {
                return IsNull((Enum) value);
            }
            if (type == typeof(int))
            {
                return IsNull((int) value);
            }
            if (type == typeof(double))
            {
                return IsNull((double) value);
            }
            if (type == typeof(DateTime))
            {
                return IsNull((DateTime) value);
            }
            if (type.IsValueType)
            {
                return false;
            }
            return (value == null);
        }

        public static bool IsNullableBaseType(Type type)
        {
            if ((!type.IsEnum && (type != typeof(int))) && ((type != typeof(double)) && (type != typeof(DateTime))))
            {
                return false;
            }
            return true;
        }

        public static Enum NullEnum(Type enumType)
        {
            return (Enum) Enum.Parse(enumType, _nullEnum.ToString());
        }

        public static DateTime NullDateTime
        {
            get
            {
                return _nullDate;
            }
        }

        public static double NullDouble
        {
            get
            {
                return _nullDouble;
            }
        }

        public static int NullInt
        {
            get
            {
                return _nullInt;
            }
        }
    }
}

