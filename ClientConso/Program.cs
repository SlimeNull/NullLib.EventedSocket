using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NullLib.EventedSocket;

namespace ClientConso
{
    class Program
    {
        static void Main(string[] args)
        {
            EventedClient client = new EventedClient();
            client.Connect(IPAddress.Parse("127.0.0.1"), 2020);
            client.DataReceived += Client_DataReceived;
            client.StartReceiveData();
            while (true)
            {
                string msg = Console.ReadLine();
                byte[] bytes = Encoding.UTF8.GetBytes(msg);
                Console.WriteLine($"{bytes.Length} sent.");
                client.BeginSendData(bytes);
            }
        }

        private static void Client_DataReceived(object sender, ClientDataReceivedArgs e)
        {
            try
            {
                Console.WriteLine($"{e.Buffer.Length}:{e.Size}:{Encoding.UTF8.GetString(e.Buffer)}");
            }
            catch
            {
                Console.WriteLine("啊出错了");
            }
        }
    }
}