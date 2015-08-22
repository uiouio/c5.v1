using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace CTPPV5.Infrastructure.Extension
{
    public static class ZipExtension
    {
        public static byte[] Zip(this byte[] data)
        {
            using (var @in = new MemoryStream(data))
            {
                using (var @out = new MemoryStream())
                {
                    using (var zipStream = new ZipOutputStream(@out))
                    {
                        zipStream.SetLevel(3);
                        zipStream.PutNextEntry(new ZipEntry(Guid.NewGuid().ToString()));
                        StreamUtils.Copy(@in, zipStream, new byte[4096]);
                        zipStream.CloseEntry();
                        zipStream.IsStreamOwner = false;
                        zipStream.Close();
                        return @out.ToArray();
                    }
                }
            }
        }

        public static byte[] Unzip(this byte[] zipped)
        {
            var unzipped = zipped;
            using (var @in = new MemoryStream(zipped))
            {
                using (var zipStream = new ZipInputStream(@in))
                {
                    var entry = zipStream.GetNextEntry();
                    while (entry != null)
                    {
                        byte[] buffer = new byte[4096];
                        using (var @out = new MemoryStream())
                        {
                            StreamUtils.Copy(zipStream, @out, buffer);
                            entry = zipStream.GetNextEntry();
                            unzipped = @out.ToArray();
                        }
                    }

                    return unzipped;
                }
            }
        }
    }
}
