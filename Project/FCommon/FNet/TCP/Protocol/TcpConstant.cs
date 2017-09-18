using FFF.Base.Util.BitConvert;

namespace FFF.Network.TCP.Protocol
{
    internal static class TcpConstant
    {

        public const int HeadTypeLength = 1;

        public const int DataLenghFieldLength = 2;

        public static readonly IBitConvert BitConvert = BitConverts.LittleEndian;

    }
}