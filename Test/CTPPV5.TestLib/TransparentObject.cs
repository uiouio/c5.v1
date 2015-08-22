using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Microsoft.CSharp.RuntimeBinder;

namespace CTPPV5.TestLib
{
    public class TransparentObject<T> : DynamicObject
    {
        private readonly T target;
        private static readonly Func<InvokeMemberBinder, IList<Type>> GetTypeArguments;

        static TransparentObject()
        {
            var type = typeof(RuntimeBinderException).Assembly.GetTypes().Where(x => x.FullName == "Microsoft.CSharp.RuntimeBinder.CSharpInvokeMemberBinder").Single();
            var dynamicMethod = new DynamicMethod("@", typeof(IList<Type>), new[] { typeof(InvokeMemberBinder) }, true);
            var il = dynamicMethod.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Castclass, type);
            il.Emit(OpCodes.Call, type.GetProperty("Microsoft.CSharp.RuntimeBinder.ICSharpInvokeOrInvokeMemberBinder.TypeArguments", BindingFlags.Instance | BindingFlags.NonPublic).GetGetMethod(true));
            il.Emit(OpCodes.Ret);
            GetTypeArguments = (Func<InvokeMemberBinder, IList<Type>>)dynamicMethod.CreateDelegate(typeof(Func<InvokeMemberBinder, IList<Type>>));
        }


        public TransparentObject(T target)
        {
            this.target = target;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var members = target.GetType().GetMember(binder.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            var member = members[0];
            if (member is PropertyInfo)
            {
                result = (member as PropertyInfo).GetValue(target, null);
                return true;
            }
            if (member is FieldInfo)
            {
                result = (member as FieldInfo).GetValue(target);
                return true;
            }
            result = null;
            return false;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var members = target.GetType().GetMember(binder.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var member = members[0];
            if (member is PropertyInfo)
            {
                (member as PropertyInfo).SetValue(target, value, null);
                return true;
            }
            if (member is FieldInfo)
            {
                (member as FieldInfo).SetValue(target, value);
                return true;
            }
            return false;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var method = target.GetType().GetMethod(binder.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (method == null) throw new MissingMemberException(string.Format("Method '{0}' not found for type '{1}'", binder.Name, typeof(T)));
            var typeArguments = GetTypeArguments(binder);
            if (typeArguments.Count > 0) method = method.MakeGenericMethod(typeArguments.ToArray());
            result = method.Invoke(target, args);
            return true;
        }
    }

    public static class TransparentObjectExtensions
    {
        public static dynamic AsTransparentObject<T>(this T o)
        {
            return new TransparentObject<T>(o);
        }
    }
}
