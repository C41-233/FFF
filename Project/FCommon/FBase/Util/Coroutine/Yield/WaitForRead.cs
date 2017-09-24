using FFF.Base.Linq;
using System;
using System.IO;

namespace FFF.Base.Util.Coroutine.Yield
{
    internal sealed class WaitForReadBuffer : ICoroutineYield, ICoroutineResult<int>
    {

        public bool IsYield { get; private set; } = true;

        public int Value { get; private set; }

        public Exception Exception { get; private set; }

        public WaitForReadBuffer(Stream stream, byte[] buffer, int offset, int len)
        {
            try
            {
                stream.BeginRead(buffer, offset, len, (ar) =>
                {
                    IsYield = false;
                    try
                    {
                        Value = stream.EndRead(ar);
                    }
                    catch (Exception e)
                    {
                        Exception = e;
                    }
                });
            }
            catch (Exception e)
            {
                Exception = e;
                IsYield = false;
            }
        }

    }

    internal sealed class WaitForRead : ICoroutineYield, ICoroutineResult<byte[]>
    {

        public bool IsYield { get; private set; } = true;

        public byte[] Value { get; private set; }

        public Exception Exception { get; private set; }

        public WaitForRead(Stream stream, int len)
        {
            try
            {
                var buffer = new byte[len];
                stream.BeginRead(buffer, (ar) =>
                {
                    IsYield = false;

                    try
                    {
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
                    }
                    catch (Exception e)
                    {
                        Exception = e;
                    }
                });
            }
            catch (Exception e)
            {
                Exception = e;
                IsYield = false;
            }
        }

    }

}
