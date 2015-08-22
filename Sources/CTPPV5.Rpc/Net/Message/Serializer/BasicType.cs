using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Infrastructure.Extension;
using CTPPV5.Infrastructure.Consts;

namespace CTPPV5.Rpc.Net.Message.Serializer
{
    public class BasicType : ISerializer
    {
        public byte Code
        {
            get { return SerializeMode.BasicType.ToByte(); }
        }

        public byte[] Serialize(object value)
        {
            byte[] buf;
            var typeCode = TypeCode.Unknown;
            if (value == null) return new byte[0];
            switch (value.GetType().FullName)
            {
                case "System.Byte" : 
                    {
                        typeCode = TypeCode.Byte;
                        buf = new byte[] { (byte)value };
                    };
                    break;
                case "System.Int16":
                    {
                        typeCode = TypeCode.Int16;
                        buf = BitConverter.GetBytes((short)value);
                    }
                    break;
                case "System.Int32":
                    {
                        typeCode = TypeCode.Int32;
                        buf = BitConverter.GetBytes((int)value);
                    }
                    break;
                case "System.Int64":
                    {
                        typeCode = TypeCode.Int64;
                        buf = BitConverter.GetBytes((long)value);
                    }
                    break;
                case "System.UInt16":
                    {
                        typeCode = TypeCode.UInt16;
                        buf = BitConverter.GetBytes((ushort)value);
                    }
                    break;
                case "System.UInt32":
                    {
                        typeCode = TypeCode.UInt32;
                        buf = BitConverter.GetBytes((uint)value);
                    }
                    break;
                case "System.UInt64":
                    {
                        typeCode = TypeCode.UInt64;
                        buf = BitConverter.GetBytes((ulong)value);
                    }
                    break;
                case "System.Single":
                    {
                        typeCode = TypeCode.Single;
                        buf = BitConverter.GetBytes((float)value);
                    }
                    break;
                case "System.Double":
                    {
                        typeCode = TypeCode.Double;
                        buf = BitConverter.GetBytes((double)value);
                    }
                    break;
                case "System.Boolean":
                    {
                        typeCode = TypeCode.Boolean;
                        buf = BitConverter.GetBytes((bool)value);
                    }
                    break;
                case "System.Char":
                    {
                        typeCode = TypeCode.Char;
                        buf = BitConverter.GetBytes((char)value);
                    }
                    break;
                case "System.String":
                    {
                        typeCode = TypeCode.String;
                        buf = Encoding.UTF8.GetBytes(value.ToString());
                    }
                    break;
                case "System.DateTime":
                    {
                        typeCode = TypeCode.DateTime;
                        buf = BitConverter.GetBytes(((DateTime)value).ToTimestamp());
                    }
                    break;
                default: throw new ArgumentException(string.Format(ExceptionMessage.UNKNOWN_TYPE_TO_SERIALIZE, value.GetType().FullName));
            }

            return new byte[] { typeCode.ToByte() }.Concat(buf);
        }

        public T DeSerialize<T>(byte[] buffer)
        {
            if (buffer == null || buffer.Length < 2)
                throw new ArgumentException(
                    string.Format(
                    ExceptionMessage.UNKNOWN_TYPE_TO_DESERIALIZE, 
                    buffer == null ? "null" : buffer.Length.ToString(), ""));

            var typeCode = buffer[0].ToEnum<TypeCode>();
            if(typeCode == TypeCode.Unknown)
                throw new ArgumentException(
                    string.Format(
                    ExceptionMessage.UNKNOWN_TYPE_TO_DESERIALIZE, buffer.Length.ToString(), buffer[0]));

            switch (typeCode)
            {
                case TypeCode.Byte: return (T)(object)(buffer[1]);
                case TypeCode.Int16: return (T)(object)BitConverter.ToInt16(buffer, 1);
                case TypeCode.Int32: return (T)(object)BitConverter.ToInt32(buffer, 1);
                case TypeCode.Int64: return (T)(object)BitConverter.ToInt64(buffer, 1);
                case TypeCode.UInt16: return (T)(object)BitConverter.ToUInt16(buffer, 1);
                case TypeCode.UInt32: return (T)(object)BitConverter.ToUInt32(buffer, 1);
                case TypeCode.UInt64: return (T)(object)BitConverter.ToUInt64(buffer, 1);
                case TypeCode.Single: return (T)(object)BitConverter.ToSingle(buffer, 1);
                case TypeCode.Double: return (T)(object)BitConverter.ToDouble(buffer, 1);
                case TypeCode.Boolean: return (T)(object)BitConverter.ToBoolean(buffer, 1);
                case TypeCode.Char: return (T)(object)BitConverter.ToChar(buffer, 1);
                case TypeCode.String: return (T)(object)Encoding.UTF8.GetString(buffer, 1, buffer.Length - 1);
                case TypeCode.DateTime: return (T)(object)BitConverter.ToInt64(buffer, 1).FromTimestamp();
                default: throw new ArgumentException(string.Format(
                   ExceptionMessage.UNKNOWN_TYPE_TO_DESERIALIZE, buffer.Length.ToString(), buffer[0]));
            }
        }

        public static bool IsBasicType(Type type)
        {
            var isSupportedType = false;
            switch (type.FullName)
            {
                case "System.Byte":
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                case "System.UInt16":
                case "System.UInt32":
                case "System.UInt64":
                case "System.Single":
                case "System.Double":
                case "System.Boolean":
                case "System.Char":
                case "System.String":
                case "System.DateTime": isSupportedType = true;
                    break;
                default: break;
            }
            return isSupportedType;
        }
    }

    public enum TypeCode
    {
        Unknown,
        Byte,
        Int16,
        Int32,
        Int64,
        UInt16,
        UInt32,
        UInt64,
        Single,
        Double,
        Boolean,
        Char,
        String,
        DateTime
    }
}
