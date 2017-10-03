using FFF.Network;
using FFF.Network.Base;
using FFF.Network.TCP;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using FFF.Base.Time;

namespace ChatServer
{
    public class Program
    {

        private class ConnectionInfo
        {
            public IConnection Connection;
            public long ConnectTime;
        }

        private class ChatInfo
        {
            public string Name;
            public IConnection Connection;
        }

        private static readonly Dictionary<ulong, ConnectionInfo> handshakes = new Dictionary<ulong, ConnectionInfo>();
        private static readonly Dictionary<string, ChatInfo> clients = new Dictionary<string, ChatInfo>();

        public static void Main(string[] args)
        {
            var server = FNet.CreateTCPServer(new TcpServerConfig()
            {
                IpAsString = "127.0.0.1",
                Port = 13000,
                MaxConnection = 5,
                PackageMaxSize = 1024,
            });
            server.OnClientConnected += conn =>
            {
                handshakes.Add(conn.ConnectionId, new ConnectionInfo()
                {
                   Connection = conn,
                   ConnectTime = FDateTime.Now.TimeStamp,
                });
            };
            server.OnClientReceive += (conn, data) =>
            {
                if (handshakes.ContainsKey(conn.ConnectionId))
                {
                    var msg = Encoding.UTF8.GetString(data);

                    var regex = new Regex(@"^handshake (a-zA-Z)+$");
                    var matches = regex.Matches(msg);
                    if (matches.Count != 1)
                    {
                        conn.Close();
                        return;
                    }
                    var groups = matches[0].Groups;
                    if (groups.Count != 1)
                    {
                        conn.Close();
                        return;
                    }

                    var name = groups[0];
                }
            };

            while (true)
            {
                server.Update();
                var now = FDateTime.Now.TimeStamp;

                foreach (var conn in handshakes.Values.ToList())
                {
                }

                Thread.Sleep(100);
            }
        }
    }
}
