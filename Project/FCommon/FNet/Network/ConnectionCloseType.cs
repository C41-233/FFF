namespace FNet.Network
{
    public enum ConnectionCloseType : int
    {

        ApplicationClose = -10000,
        UnrecognizedPackage = -10001,
        PackageSizeTooLarge = -10002,
        KeepAliveTimeout = -10003,

        ConnectionAborted = 10053,
        ConnectionReset = 10054,

    }
}
