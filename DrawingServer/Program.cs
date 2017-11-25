using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DrawingServer
{
    class Program
    {
        static Server server = new Server(5432);
        static void Main(string[] args)
        {
            while (true)
            {
                // add client to server
                TcpClient client = server.newClient();
                // connect new client in seperate thread
                Task.Run(() =>
                {
                    Console.WriteLine("user joined");
                    // while thread is running, send message, else exit loop.
                    while (true)
                    {
                        try
                        {
                            string message = server.recieve(client);
                            Console.WriteLine("recieved "+message);
                            server.sendToAll(message);
                        }
                        catch
                        {
                            break;
                        }
                    }
                });
            }
            // wait for client
        }
    }
}
