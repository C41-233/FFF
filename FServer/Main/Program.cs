using FFF.Server.Application;
using System;
using System.IO;
using FFF.Server.Application.TimeTick;
using FNet.Network;
using FNet.TCP;

namespace Main
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            FApplication.Run<MainApplication>(args, new FApplicationConfig()
            {
                Tick= 1000,
            });
            Console.Read();
        }
    }

    class MainApplication : IFApplication
    {

        private IConnection client;

        void IFApplication.OnInit(string[] args)
        {
            var config = new FTcpServerConfig()
            {
                IpAsString = "127.0.0.1"
            };
            var server = new FTcpServer(config);
            server.OnClientConnected += conn =>
            {
                Console.WriteLine("connected");
                client = conn;
                client.BeginReceive();
            };
            server.OnClientDisconnected += conn =>
            {

                Console.WriteLine("disconnected");
            };
            server.BeginAccept();
        }

        void IFApplication.OnDestroy()
        {
        }

        void IFApplication.OnTick()
        {
            client?.Flush();
        }

    }
}
