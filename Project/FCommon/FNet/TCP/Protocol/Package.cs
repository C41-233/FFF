namespace FFF.Network.TCP.Protocol
{

    public sealed class TcpPingPackage
    {

        public readonly PackageType Type = PackageType.Ping;

    }

    public sealed class TcpPongPackage
    {

        public readonly PackageType Type = PackageType.Pong;

    }

    public sealed class TcpDataPackage
    {

        public readonly PackageType Type = PackageType.Data;
        public ushort Length => (ushort)Data.Length;
        public byte[] Data;

    }

}
