using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NullLib.EventedSocket
{
    public class EventedListener
    {
        IPEndPoint localEP;
        TcpListener server;

        bool continueAccept;

        public EventedListener(int port)
        {
            this.localEP = new IPEndPoint(IPAddress.Any, port);
            this.server = new TcpListener(localEP);
        }
        public EventedListener(IPEndPoint localEP)
        {
            this.localEP = localEP;
            this.server = new TcpListener(localEP);
        }
        public EventedListener(IPAddress localaddr, int port)
        {
            this.localEP = new IPEndPoint(localaddr, port);
            this.server = new TcpListener(localEP);
        }

        public bool ExclusiveAddressUse { get => server.ExclusiveAddressUse; set => server.ExclusiveAddressUse = value; }
        public EndPoint LocalEndpoint { get => localEP; }
        public TcpListener Listener { get => server; }
        public Socket BaseSocket { get => server.Server; }

        public bool IsAcceptEvented { get => continueAccept; }

        private async void EventAcceptClientLoopAsync()
        {
            await Task.Run(() =>
            {
                while (continueAccept)
                    OnClientConnected(AcceptClient());
            });
        }

        public EventedClient AcceptClient()
        {
            TcpClient tcpClient;

            try
            {
                tcpClient = server.AcceptTcpClient();
                return new EventedClient(tcpClient);
            }
            catch(InvalidOperationException ex)
            {
                throw new InvalidOperationException("The listener has not been started with a call to NullLib.EventedSocket.EventedListener.Start.", ex);
            }
            catch(SocketException ex)
            {
                throw ex;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public IAsyncResult BeginAcceptClient(AsyncCallback callback, object state)
        {
            return server.BeginAcceptSocket(callback, state);
        }
        public EventedClient EndAcceptClient(IAsyncResult asyncResult)
        {
            return new EventedClient(server.EndAcceptTcpClient(asyncResult));
        }
        public void AllowNatTraversal(bool allowed)
        {
            server.AllowNatTraversal(allowed);
        }

        public void StartAcceptClient()
        {
            continueAccept = true;
            EventAcceptClientLoopAsync();
        }
        public void StopAcceptClient()
        {
            continueAccept = false;
        }
        
        public void Start()
        {
            server.Start();
        }
        public void Start(int backlog)
        {
            server.Start(backlog);
        }
        public void Stop()
        {
            server.Stop();
        }
        public bool Pending()
        {
            return server.Pending();
        }

        ~EventedListener()
        {
            continueAccept = false;
        }

        private void OnClientConnected(EventedClient client)
        {
            if (ClientConnected != null)
                ClientConnected.Invoke(this, new ClientConnectedEventArgs(client));
        }

        public event EventHandler<ClientConnectedEventArgs> ClientConnected;
    }
    public class EventedClient : IDisposable
    {
        TcpClient client;
        NetworkStream stream;

        bool continueReceive;

        public EventedClient()
        {
            client = new TcpClient();
        }
        public EventedClient(TcpClient client)
        {
            if (client == null)
                throw new ArgumentNullException("client");

            this.client = client;
            stream = client.GetStream();
        }

        public int ReceiveTimeout { get => client.ReceiveTimeout; set => client.ReceiveTimeout = value; }
        public int ReceiveBufferSize { get => client.ReceiveBufferSize; set => ReceiveBufferSize = value; }
        public bool NoDelay { get => client.NoDelay; set => client.NoDelay = value; }
        public LingerOption LingerOption { get => client.LingerState; set => client.LingerState = value; }
        public bool ExclusiveAddressUse { get => client.ExclusiveAddressUse; set => client.ExclusiveAddressUse = value; }
        public bool Connected { get => client.Connected; }
        public TcpClient Client { get => client; }
        public Socket BaseSocket { get => client.Client; }
        public int Available { get => client.Available; }
        public int SendBufferSize { get => client.SendBufferSize; set => client.SendBufferSize = value; }
        public int SendTimeout { get => client.SendTimeout; set => client.SendTimeout = value; }

        public bool IsReceiveEvented { get => continueReceive; }

        public IAsyncResult BeginConnect(IPAddress address, int port, AsyncCallback callback, object state)
        {
            AsyncCallback connectCallback = (rst) => stream = client.GetStream();
            connectCallback += callback;

            return client.BeginConnect(address, port, connectCallback, state);
        }
        public void EndConnect(IAsyncResult result)
        {
            client.EndConnect(result);
        }
        public void Connect(IPAddress address, int port)
        {
            client.Connect(address, port);
            stream = client.GetStream();
        }

        public void SendData(byte[] buffer, int offset, int size)
        {
            byte[] head = BitConverter.GetBytes(size);
            stream.Write(head, 0, 4);
            stream.Write(buffer, offset, size);
        }
        public void SendData(byte[] buffer, int offset)
        {
            int size = buffer.Length - offset;
            SendData(buffer, offset, size);
        }
        public void SendData(byte[] buffer)
        {
            int size = buffer.Length;
            SendData(buffer, 0, size);
        }
        public async void SendDataAsync(byte[] buffer, int offset, int size)
        {
            await Task.Run(() =>
            {
                SendData(buffer, offset, size);
            });
        }
        public IAsyncResult BeginSendData(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
        {
            return stream.BeginWrite(buffer, offset, size, callback, state);
        }
        public IAsyncResult BeginSendData(byte[] buffer, int offset, AsyncCallback callback, object state)
        {
            int size = buffer.Length - offset;
            return stream.BeginWrite(buffer, offset, size, callback, state);
        }
        public IAsyncResult BeginSendData(byte[] buffer, AsyncCallback callback, object state)
        {
            int size = buffer.Length;
            return stream.BeginWrite(buffer, 0, size, callback, state);
        }
        public void BeginSendData(byte[] buffer, int offset, int size)
        {
            SendDataAsync(buffer, offset, size);
        }
        public void BeginSendData(byte[] buffer, int offset)
        {
            int size = buffer.Length - offset;
            SendDataAsync(buffer, offset, size);
        }
        public void BeginSendData(byte[] buffer)
        {
            int size = buffer.Length;
            SendDataAsync(buffer, 0, size);
        }
        public void EndSendData(IAsyncResult result)
        {
            stream.EndWrite(result);
        }
        
        public byte[] ReceiveData()
        {
            int headRecv = 0, bodyRecv = 0;
            byte[] head = new byte[4], body = null;
            try
            {
                do
                    headRecv += stream.Read(head, headRecv, 4 - headRecv);
                while (headRecv != 4);

                int bodySize = BitConverter.ToInt32(head, 0);
                body = new byte[bodySize];

                do
                    bodyRecv += stream.Read(body, bodyRecv, bodySize - bodyRecv);
                while (bodyRecv != bodySize);

                return body;
            }
            catch (ArgumentOutOfRangeException)
            {
                OnErrorReceived(body, bodyRecv);
                return null;
            }
            catch (System.IO.IOException)
            {
                OnDisconnected();
                return null;
            }
            catch (ObjectDisposedException ex1)
            {
                throw ex1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task ReceiveDataAsync(AsyncCallback callback, object state)
        {
            Task<byte[]> task = new Task<byte[]>((_) => ReceiveData(), state);
            task.Start();
            await task;
            callback.Invoke(task);  // 好丑... 之后想办法把这里改好看一点
        }

        public IAsyncResult BeginReceiveData(AsyncCallback callback, object state)
        {
            return ReceiveDataAsync(callback, state);
        }
        public byte[] EndReceiveData(IAsyncResult result)
        {
            if (result is Task<byte[]>)
                return (result as Task<byte[]>).Result;
            else
                return null;
        }

        public void Dispose()
        {
            client.Dispose();
        }

        private async void ReceiveDataLoopAsync()
        {
            await Task.Run(()=>
            {
                while (continueReceive)
                {
                    byte[] data = ReceiveData();
                    if (data != null)
                        OnDataReceived(data, data.Length);
                }
            });
        }
        public void StartReceiveData()
        {
            continueReceive = true;
            ReceiveDataLoopAsync();
        }
        public void StopReceiveData()
        {
            continueReceive = false;
        }

        private void OnDisconnected()
        {
            continueReceive = false;

            if (Disconnected != null)
                Disconnected.Invoke(this, new ClientDisconnectedEventArgs(this));
        }
        private void OnDataReceived(byte[] buffer, int size)
        {
            if (DataReceived != null)
                DataReceived.Invoke(this, new ClientDataReceivedEventArgs(this, buffer, size));
        }
        private void OnErrorReceived(byte[] buffer, int size)
        {
            if (ErrorReceived != null)
                ErrorReceived.Invoke(this, new ClientErrorReceivedEventArgs(this, buffer, size));
        }

        public event EventHandler<ClientDisconnectedEventArgs> Disconnected;
        public event EventHandler<ClientDataReceivedEventArgs> DataReceived;
        public event EventHandler<ClientErrorReceivedEventArgs> ErrorReceived;
    }
    public class SocketStateObject
    {
        public Socket WorkSocket = null;
        public bool IsLength = true;
        public byte[] Buffer = new byte[4];

        public SocketStateObject()
        {
            WorkSocket = null;
            IsLength = true;
            Buffer = new byte[4];
        }
    }

    public class ClientConnectedEventArgs : EventArgs
    {
        public EventedClient Client;
        public ClientConnectedEventArgs(EventedClient client)
        {
            this.Client = client;
        }
    }
    public class ClientDisconnectedEventArgs : EventArgs
    {
        public EventedClient Client;
        public ClientDisconnectedEventArgs(EventedClient client)
        {
            this.Client = client;
        }
    }
    public class ClientDataReceivedEventArgs : EventArgs
    {
        public EventedClient Client;
        public byte[] Buffer;
        public int Size;
        public ClientDataReceivedEventArgs(EventedClient client, byte[] buffer, int size)
        {
            this.Client = client;
            this.Buffer = buffer;
            this.Size = size;
        }
    }
    public class ClientErrorReceivedEventArgs : EventArgs
    {
        public EventedClient Client;
        public byte[] Buffer;
        public int Size;
        public ClientErrorReceivedEventArgs(EventedClient client, byte[] buffer, int size)
        {
            this.Client = client;
            this.Buffer = buffer;
            this.Size = size;
        }
    }
}
