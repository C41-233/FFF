using FNet.Network;

namespace FNet.TCP.Protocol
{

    public class FTcpPingPackage
    {

        public readonly byte Type = PackageType.Ping;

    }

    public class FTcpPongPackage
    {

        public readonly byte Type = PackageType.Pong;

    }

    public class FTcpDataPackage
    {

        public readonly byte Type = PackageType.Data;
        public ushort Length => (ushort)Data.Length;
        public byte[] Data;

    }

}
