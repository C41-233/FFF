namespace FFF.Network.TCP.Protocol
{

    public class TcpPingPackage
    {

        public readonly PackageType Type = PackageType.Ping;

    }

    public class TcpPongPackage
    {

        public readonly PackageType Type = PackageType.Pong;

    }

    public class TcpDataPackage
    {

        public readonly PackageType Type = PackageType.Data;
        public ushort Length => (ushort)Data.Length;
        public byte[] Data;

    }

}
