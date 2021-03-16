using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NullLib.EventedSocket;

namespace ServerConso
{
    class Program
    {
        static void Main(string[] args)
        {
            EventedListener listener = new EventedListener(2020);
            listener.Start();

            listener.ClientConnected += Listener_ClientConnected;

            listener.StartAcceptClient();
            while (true)
                Console.ReadKey(true);
        }

        private static void Listener_ClientConnected(object sender, ClientConnectedEventArgs e)
        {
            EventedClient client = e.Client;
            client.DataReceived += Client_DataReceived;
            client.StartReceiveData();
        }

        private static void Client_DataReceived(object sender, ClientDataReceivedEventArgs e)
        {
            e.Client.BeginSendData(e.Buffer, 0, e.Size);
        }
    }
}
