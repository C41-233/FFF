using FFF.Network.Base;
using FFF.Network.TCP;
using FFF.Server.Application;
using System;
using System.Net.Http;
using System.Text;
using FFF.Base.Util;
using FFF.Network;

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

        int client = 0;

        void IApplication.OnInit(string[] args)
        {
            server = FNet.CreateTCPServer(new TcpServerConfig()
            {
                KeepAlive = 5000,
            });

            server.OnClientConnected += conn =>
            {
                Console.WriteLine($"connected: {conn.IP}:{conn.Port}");
                client++;
            };
            server.OnClientDisconnected += (conn, type) =>
            {
                Console.WriteLine($"disconnected: {type}");
            };
            server.OnClientReceive += (conn, data) =>
            {
                var msg = Encoding.UTF8.GetString(data);
                Console.WriteLine($"receive: {msg}");

                var send = Encoding.UTF8.GetBytes("pong");
                conn.Send(send);
            };
            server.OnServerClosed += server =>
            {
                Console.WriteLine("Server closed.");
            };
            server.BeginAccept();
        }

        void IApplication.OnDestroy()
        {
        }

        void IApplication.OnTick()
        {
            server.Update();
            if (client == 3)
            {
                server.Close();
            }
        }

    }
}
