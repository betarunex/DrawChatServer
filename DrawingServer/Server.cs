using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DrawingServer
{
    class Server
    {
        int port;
        TcpListener listener;
        public List<TcpClient> clients = new List<TcpClient>();

        public Server(int port = 5432)
        {
            this.port = port;
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
        }

        public void sendToAll(string message)
        {
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(message);
            for (int x = 0; x < clients.Count; x++)
            {
                try
                {
                    clients[x].GetStream().Write(bytes, 0, bytes.Length);
                }
                catch
                {
                    clients.RemoveAt(x);
                    if (x - 1 < 0)
                    {
                        x--;
                    }
                }
            }
        }

        public TcpClient newClient()
        {
            TcpClient client = listener.AcceptTcpClient();
            clients.Add(client);
            return client;
        }
        /*
        public void send(string message, TcpClient client)
        {
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(message);
            client.GetStream().Write(bytes, 0, bytes.Length);
        }
        */

        public string recieve(TcpClient client)
        {
            byte[] bytes = new byte[client.ReceiveBufferSize];
            int toRead = client.GetStream().Read(bytes, 0, client.ReceiveBufferSize);
            return ASCIIEncoding.ASCII.GetString(bytes, 0, toRead);
        }
    }
}
