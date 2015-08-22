using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Infrastructure.Extension
{
    public static class ArrayExtension
    {
        public static T[] Copy<T>(this T[] array, int index, int length)
        {
            var target = array;
            if (index < length && length > 0)
            {
                target = new T[length];
                Array.Copy(array, index, target, 0, length);
            }
            return target;
        }

        public static T[] Concat<T>(this T[] array, T[] target)
        {
            var @new = array;
            if (target != null && target.Length > 0)
            {
                @new = new T[array.Length + target.Length];
                Array.Copy(array, 0, @new, 0, array.Length);
                Array.Copy(target, 0, @new, array.Length, target.Length);
            }
            return @new;
        }

        public static void Foreach<T>(this T[] array, Action<T> action)
        {
            if (array != null) foreach (var item in array) action(item);
        }
    }
}
