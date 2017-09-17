using FFF.Server.Application;
using FNet.Network;
using FNet.TCP;
using System;
using System.Text;

namespace Main
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Application.Run<MainApplication>(args, new ApplicationConfig()
            {
                Tick= 50,
            });
            Console.Read();
        }
    }

    class MainApplication : IApplication
    {

        private IServer server;

        void IApplication.OnInit(string[] args)
        {
            var config = new TcpServerConfig()
            {
                IpAsString = "127.0.0.1",
                SendImmediately = false,
                MaxConnection = 1,
                KeepAlive = 5000,
            };
            this.server = new TcpServer(config);
            server.OnClientConnected += conn =>
            {
                Console.WriteLine("connected");
                conn.Send(Encoding.Default.GetBytes("hello"));
                //Coroutines.StartCoroutineAfter(5000, conn.Close);
            };
            server.OnClientDisconnected += (conn, err) =>
            {
                Console.WriteLine("disconnected err="+err);
            };
            server.OnClientReceive += (conn, data) =>
            {
                var rec = Encoding.Default.GetString(data);
                Console.WriteLine("receive " + rec);
                conn.Send(Encoding.Default.GetBytes("reply "+rec));
            };
            server.BeginAccept();
        }

        void IApplication.OnDestroy()
        {
        }

        void IApplication.OnTick()
        {
            server.Update();
        }

    }
}
