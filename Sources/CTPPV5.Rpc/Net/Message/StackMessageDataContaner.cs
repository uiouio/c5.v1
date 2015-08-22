using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Rpc.Net.Message
{
    public class StackMessageDataContaner : IMessageDataContainer
    {
        private Stack<byte[]> dataStack = new Stack<byte[]>();
        public byte[] Take()
        {
            byte[] buffer = new byte[0];
            if (dataStack.Count > 0)
                buffer = dataStack.Pop();
            return buffer;
        }

        public void Push(byte[] data)
        {
            if (data != null)
                dataStack.Push(data);
        }
    }
}
