using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using CTPPV5.Rpc.Net.Message.Serializer;

namespace CTPPV5.Rpc.Test
{
    [TestFixture]
    public class BasicTypeTest
    {
        [Test]
        public void Test()
        {
            byte a = 1;
            short b = 1;
            int c = -1;
            long d = 1;
            ushort e = 1;
            uint f = 1;
            ulong g = 1;
            float h = 1.1f;
            double i = 1.222;
            bool j = true;
            char k = 'a';
            string l = "aaa";
            DateTime m = new DateTime(2015,1,1,1,1,1);

            var encoder = new BasicType();
            Assert.IsTrue(BasicType.IsBasicType(a.GetType()));
            Assert.AreEqual(a, encoder.DeSerialize<byte>(encoder.Serialize(a)));
            Assert.IsTrue(BasicType.IsBasicType(b.GetType()));
            Assert.AreEqual(b, encoder.DeSerialize<short>(encoder.Serialize(b)));
            Assert.IsTrue(BasicType.IsBasicType(c.GetType()));
            Assert.AreEqual(c, encoder.DeSerialize<int>(encoder.Serialize(c)));
            Assert.IsTrue(BasicType.IsBasicType(d.GetType()));
            Assert.AreEqual(d, encoder.DeSerialize<long>(encoder.Serialize(d)));
            Assert.IsTrue(BasicType.IsBasicType(e.GetType()));
            Assert.AreEqual(e, encoder.DeSerialize<ushort>(encoder.Serialize(e)));
            Assert.IsTrue(BasicType.IsBasicType(f.GetType()));
            Assert.AreEqual(f, encoder.DeSerialize<uint>(encoder.Serialize(f)));
            Assert.IsTrue(BasicType.IsBasicType(g.GetType()));
            Assert.AreEqual(g, encoder.DeSerialize<ulong>(encoder.Serialize(g)));
            Assert.IsTrue(BasicType.IsBasicType(h.GetType()));
            Assert.AreEqual(h, encoder.DeSerialize<float>(encoder.Serialize(h)));
            Assert.IsTrue(BasicType.IsBasicType(i.GetType()));
            Assert.AreEqual(i, encoder.DeSerialize<double>(encoder.Serialize(i)));
            Assert.IsTrue(BasicType.IsBasicType(j.GetType()));
            Assert.AreEqual(j, encoder.DeSerialize<bool>(encoder.Serialize(j)));
            Assert.IsTrue(BasicType.IsBasicType(k.GetType()));
            Assert.AreEqual(k, encoder.DeSerialize<char>(encoder.Serialize(k)));
            Assert.IsTrue(BasicType.IsBasicType(l.GetType()));
            Assert.AreEqual(l, encoder.DeSerialize<string>(encoder.Serialize(l)));
            Assert.IsTrue(BasicType.IsBasicType(m.GetType()));
            Assert.AreEqual(m, encoder.DeSerialize<DateTime>(encoder.Serialize(m)));
            Assert.IsFalse(BasicType.IsBasicType(typeof(object)));
            Assert.Throws(typeof(ArgumentException), () => encoder.Serialize(new List<int>()));
        }
    }
}
