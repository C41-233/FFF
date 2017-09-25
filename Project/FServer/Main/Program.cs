using FFF.Network;
using FFF.Network.Base;
using FFF.Network.TCP;
using FFF.Server.Application;
using System;
using System.Text;
using FFF.Network.TCP.Server;

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
            server = FNet.CreateTcpServer(new TcpServerConfig()
            {
                KeepAlive = 5000,
                SendImmediately = true,
            });

            server.OnClientConnected += conn =>
            {
                Console.WriteLine($"connected: {conn.ConnectionId} {conn.IP}:{conn.Port}");
                client++;
            };
            server.OnClientDisconnected += (conn, type) =>
            {
                Console.WriteLine($"disconnected: {conn.ConnectionId} {type}");
            };
            server.OnClientReceive += (conn, data) =>
            {
                var msg = Encoding.UTF8.GetString(data);
                Console.WriteLine($"receive: {conn.ConnectionId} {msg}");

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
