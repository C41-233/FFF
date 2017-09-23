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

    }
}
