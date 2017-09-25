using System;
using System.IO;

namespace FFF.Base.Linq
{
    public static class StreamExtension
    {

        public static IAsyncResult BeginRead(this Stream stream, byte[] buffer, int offset, int count, AsyncCallback callback)
        {
            return stream.BeginRead(buffer, offset, count, callback, stream);
        }

        public static IAsyncResult BeginRead(this Stream stream, byte[] buffer, AsyncCallback callback)
        {
            return stream.BeginRead(buffer, 0, buffer.Length, callback, stream);
        }

        public static void Write(this Stream stream, byte[] bs)
        {
            stream.Write(bs, 0, bs.Length);
        }

    }
}
