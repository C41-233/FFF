using FFF.Base.Linq;
using System.IO;

namespace FFF.Base.Util.Coroutine.Yield
{
    internal class WaitForRead : ICoroutineYield, IWaitForReadResult
    {

        public bool IsYield { get; private set; } = true;

        public int BytesRead { get; private set; }

        public WaitForRead(Stream stream, byte[] buffer, int offset, int len)
        {
            stream.BeginRead(buffer, offset, len, (ar) =>
            {
                BytesRead = stream.EndRead(ar);
                IsYield = false;
            });
        }

    }

    public interface IWaitForReadResult
    {
        
        int BytesRead { get; }
                
    }
}
