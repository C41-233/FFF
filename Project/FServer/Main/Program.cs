using FFF.Server.Application;
using System;
using System.Collections;
using System.Threading;
using FFF.Base.Util.Coroutine.Yield;
using FFF.Server.Application.Coroutine;
using FFF.Server.Application.Tick;
using FFF.Server.Application.Timer;
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
            server.OnClientDisconnected += (conn, err) =>
            {

                Console.WriteLine("disconnected err="+err);
            };
            server.BeginAccept();

            Timers.StartTimerAfter(5000, () =>
            {
                Console.WriteLine("timer  "+TimeTick.MillisecondsFromStart);
                Timers.StartTimerAfter(1000, () =>
                {
                    Console.WriteLine("timer  " + TimeTick.MillisecondsFromStart);
                });
            });
        }

        void IApplication.OnDestroy()
        {
        }

        void IApplication.OnTick()
        {
            client?.Flush();
        }

    }
}
