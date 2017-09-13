namespace FNet.TCP.Protocol
{

    public class FTcpPingPackage
    {

        public readonly PackageType Type = PackageType.Ping;

    }

    public class FTcpPongPackage
    {

        public readonly PackageType Type = PackageType.Pong;

    }

    public class FTcpDataPackage
    {

        public readonly PackageType Type = PackageType.Data;
        public ushort Length => (ushort)Data.Length;
        public byte[] Data;

    }

}
