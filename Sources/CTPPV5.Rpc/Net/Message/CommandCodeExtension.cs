using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Infrastructure.Util;

namespace CTPPV5.Rpc.Net.Message
{
    public static class CommandCodeExtension
    {
        static HashSet<CommandCode> secureHashSet = new HashSet<CommandCode>();
        static DoubleCheckLock doubleCheckLock = new DoubleCheckLock();
        public static bool IsSecure(this CommandCode commandCode)
        {
            using (var locker = doubleCheckLock.Accquire(
                () => secureHashSet.Contains(commandCode)))
            {
                if (locker.Locked)
                {
                    var newSet = new HashSet<CommandCode>();
                    var enumType = commandCode.GetType();
                    foreach (var val in Enum.GetValues(enumType))
                    {
                        var field = enumType.GetField(val.ToString());
                        if (Attribute.IsDefined(field, typeof(SecureAttribute)))
                        {
                            newSet.Add((CommandCode)Enum.Parse(
                                typeof(CommandCode), val.ToString()));
                        }
                    }
                    secureHashSet = newSet;
                }
                return secureHashSet.Contains(commandCode);
            }
        }
    }
}
