using FFF.Server.Application;
using System;
using System.Collections;
using System.Threading;
using FFF.Base.Util.Coroutine.Yield;
using FFF.Server.Application.Tick;
using FNet.Network;
using FNet.TCP;

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

        private IConnection client;

        void IApplication.OnInit(string[] args)
        {
            var config = new TcpServerConfig()
            {
                IpAsString = "127.0.0.1"
            };
            var server = new TcpServer(config);
            server.OnClientConnected += conn =>
            {
                Console.WriteLine("connected");
                client = conn;
                client.BeginReceive();
            };
            server.OnClientDisconnected += (conn, err) =>
            {

                Console.WriteLine("disconnected err="+err);
            };
            server.BeginAccept();

            Coroutines.StartCoroutine(Do);
        }

        IEnumerator Do()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                Console.WriteLine(TimeTick.MillisecondsFromStart);
            }
        }

        void IApplication.OnDestroy()
        {
        }

        void IApplication.OnTick()
        {
            //client?.Flush();
        }

    }
}
