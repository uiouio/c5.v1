using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Infrastructure.Extension
{
    public static class EnumExtension
    {
        public static TEnum ToEnum<TEnum>(this int val)
            where TEnum : struct
        {
            var parsed = (TEnum)Enum.ToObject(typeof(TEnum), val);
            return parsed.ToString().IsNumeric() ? default(TEnum) : parsed;
        }

        public static TEnum ToEnum<TEnum>(this short val)
            where TEnum : struct
        {
            return ((int)val).ToEnum<TEnum>();
        }

        public static TEnum ToEnum<TEnum>(this byte val)
            where TEnum : struct
        {
            return ((int)val).ToEnum<TEnum>();
        }

        public static TEnum ToEnum<TEnum>(this string val)
            where TEnum : struct
        {
            var parsedVal = default(TEnum);
            if (Enum.TryParse<TEnum>(val, out parsedVal)
                && Enum.IsDefined(typeof(TEnum), parsedVal))
            {
            }
            else parsedVal = default(TEnum);
            return parsedVal;
        }

        public static byte ToByte(this Enum @enum)
        {
            return Convert.ToByte(@enum);
        }

        public static int ToInt32(this Enum @enum)
        {
            return Convert.ToInt32(@enum);
        }
    }
}
