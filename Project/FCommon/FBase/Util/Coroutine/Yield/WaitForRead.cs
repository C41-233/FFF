using FFF.Base.Linq;
using System;
using System.IO;

namespace FFF.Base.Util.Coroutine.Yield
{
    internal class WaitForReadBuffer : ICoroutineYield, ICoroutineResult<int>
    {

        public bool IsYield { get; private set; } = true;

        public int Value { get; private set; }

        public WaitForReadBuffer(Stream stream, byte[] buffer, int offset, int len)
        {
            stream.BeginRead(buffer, offset, len, (ar) =>
            {
                Value = stream.EndRead(ar);
                IsYield = false;
            });
        }

    }

    internal class WaitForRead : ICoroutineYield, ICoroutineResult<byte[]>
    {

        public bool IsYield { get; private set; } = true;

        public byte[] Value { get; private set; }

        public WaitForRead(Stream stream, int len)
        {
            var buffer = new byte[len];
            stream.BeginRead(buffer, (ar) =>
            {
                IsYield = false;

                var nread = stream.EndRead(ar);
                if (nread < 0)
                {
                    Value = new byte[0];
                }
                else
                {
                    Array.Resize(ref buffer, nread);
                    Value = buffer;
                }
            });
        }

    }

}
