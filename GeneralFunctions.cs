using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;

namespace JJ2AnimLib
{
    internal class GeneralFunctions
    {

        [DllImport("zlib1.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "uncompress", CharSet = CharSet.Auto)]
        public static extern int UncompressByteArray(byte[] dest, ref long destLen, byte[] src, long srcLen);

        public static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        public static byte[] Unzip(byte[] bytes, int index, int count)
        {
            using (var msi = new MemoryStream(bytes, index, count))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    //gs.CopyTo(mso);
                    CopyTo(gs, mso);
                }

                return mso.ToArray();
            }
        }


        public static byte[] Decompress(Stream source)
        {
            using (var gzip = new GZipStream(source, CompressionMode.Decompress))
            {
                using (var decompressed = new MemoryStream())
                {
#if NETSTANDARD2_0_OR_GREATER
                    gzip.CopyTo(decompressed);
#endif
                    return decompressed.ToArray();
                }
            }
        }

      public  static byte[] Decompress(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                return Decompress(ms);
            }
        }

    }
}
