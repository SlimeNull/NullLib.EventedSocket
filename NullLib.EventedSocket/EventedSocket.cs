using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace NullLib.EventedSocket
{
    public static class EventedSocket
    {
        public static void SendData(Socket target, byte[] data, int offset, int length)
        {
            target.Send(BitConverter.GetBytes(length));
            target.Send(data, offset, length, SocketFlags.None);
        }
        public static void SendData(Socket target, byte[] data, int offset)
        {
            target.Send(BitConverter.GetBytes(data.Length - offset));
            target.Send(data, offset, SocketFlags.None);
        }
        public static void SendData(Socket target, byte[] data)
        {
            target.Send(BitConverter.GetBytes(data.Length));
            target.Send(data);
        }
        public static void BeginSendData(Socket target, byte[] data, int offset, int length, AsyncCallback callback, object state)
        {
            target.Send(BitConverter.GetBytes(length));
            target.BeginSend(data, offset, length, SocketFlags.None, callback, state);
        }
        public static void BeginSendData(Socket target, byte[] data, int offset, AsyncCallback callback, object state)
        {
            int length = data.Length - offset;
            target.Send(BitConverter.GetBytes(length));
            target.BeginSend(data, offset, length, SocketFlags.None, callback, state);
        }
        public static void BeginSendData(Socket target, byte[] data, AsyncCallback callback, object state)
        {
            int length = data.Length;
            target.Send(BitConverter.GetBytes(length));
            target.BeginSend(data, 0, length, SocketFlags.None, callback, state);
        }
        public static void BeginSendData(Socket target, byte[] data, int offset, int length)
        {
            target.Send(BitConverter.GetBytes(length));
            target.BeginSend(data, offset, length, SocketFlags.None, (_) => target.EndSend(_), null);
        }
        public static void BeginSendData(Socket target, byte[] data, int offset)
        {
            int length = data.Length - offset;
            target.Send(BitConverter.GetBytes(length));
            target.BeginSend(data, 0, length, SocketFlags.None, (_) => target.EndSend(_), null);
        }
        public static void BeginSendData(Socket target, byte[] data)
        {
            int length = data.Length;
            target.Send(BitConverter.GetBytes(length));
            target.BeginSend(data, 0, length, SocketFlags.None, (_) => target.EndSend(_), null);
        }

        public static void BoardcastData(IEnumerable<Socket> targets, byte[] data, int offset, int length)
        {
            foreach (Socket socket in targets)
                EventedSocket.SendData(socket, data, offset, length);
        }
        public static void BoardcastData(IEnumerable<Socket> targets, byte[] data, int offset)
        {
            foreach (Socket socket in targets)
                EventedSocket.SendData(socket, data, offset);
        }
        public static void BoardcastData(IEnumerable<Socket> targets, byte[] data)
        {
            foreach (Socket socket in targets)
                SendData(socket, data);
        }
        public static void BeginBoardcastData(IEnumerable<Socket> targets, byte[] data, int offset, int length, AsyncCallback callback, object state)
        {
            foreach (Socket socket in targets)
                BeginSendData(socket, data, offset, length, callback, state);
        }
        public static void BeginBoardcastData(IEnumerable<Socket> targets, byte[] data, int offset, AsyncCallback callback, object state)
        {
            foreach (Socket socket in targets)
                BeginSendData(socket, data, offset, callback, state);
        }
        public static void BeginBoardcastData(IEnumerable<Socket> targets, byte[] data, AsyncCallback callback, object state)
        {
            foreach (Socket socket in targets)
                BeginSendData(socket, data, callback, state);
        }
        public static void BeginBoardcastData(IEnumerable<Socket> targets, byte[] data, int offset, int length)
        {
            foreach (Socket socket in targets)
                BeginSendData(socket, data, offset, length);
        }
        public static void BeginBoardcastData(IEnumerable<Socket> targets, byte[] data, int offset)
        {
            foreach (Socket socket in targets)
                BeginSendData(socket, data, offset);
        }
        public static void BeginBoardcastData(IEnumerable<Socket> targets, byte[] data)
        {
            foreach (Socket socket in targets)
                BeginSendData(socket, data);
        }

        public static bool TrySendData(Socket target, byte[] data, int offset, int length)
        {
            try { SendData(target, data, offset, length); return true; } catch { return false; }
        }
        public static bool TrySendData(Socket target, byte[] data, int offset)
        {
            try { SendData(target, data, offset); return true; } catch { return false; }
        }
        public static bool TrySendData(Socket target, byte[] data)
        {
            try { SendData(target, data); return true; } catch { return false; }
        }
        public static bool TryBeginSendData(Socket target, byte[] data, int offset, int length, AsyncCallback callback, object state)
        {
            try { BeginSendData(target, data, offset, length, callback, state); return true; } catch { return false; }
        }
        public static bool TryBeginSendData(Socket target, byte[] data, int offset, AsyncCallback callback, object state)
        {
            try { BeginSendData(target, data, offset, callback, state); return true; } catch { return false; }
        }
        public static bool TryBeginSendData(Socket target, byte[] data, AsyncCallback callback, object state)
        {
            try { BeginSendData(target, data, callback, state); return true; } catch { return false; }
        }
        public static bool TryBeginSendData(Socket target, byte[] data, int offset, int length)
        {
            try { BeginSendData(target, data, offset, length); return true; } catch { return false; }
        }
        public static bool TryBeginSendData(Socket target, byte[] data, int offset)
        {
            try { BeginSendData(target, data, offset); return true; } catch { return false; }
        }
        public static bool TryBeginSendData(Socket target, byte[] data)
        {
            try { BeginSendData(target, data); return true; } catch { return false; }
        }

        public static bool TryBoardcastData(IEnumerable<Socket> targets, byte[] data, int offset, int length)
        {
            try { BeginBoardcastData(targets, data, offset, length); return true; } catch { return false; }
        }
        public static bool TryBoardcastData(IEnumerable<Socket> targets, byte[] data, int offset)
        {
            try { BeginBoardcastData(targets, data, offset); return true; } catch { return false; }
        }
        public static bool TryBoardcastData(IEnumerable<Socket> targets, byte[] data)
        {
            try { BeginBoardcastData(targets, data); return true; } catch { return false; }
        }
        public static bool TryBeginBoardcastData(IEnumerable<Socket> targets, byte[] data, int offset, int length, AsyncCallback callback, object state)
        {
            try { BeginBoardcastData(targets, data, offset, length, callback, state); return true; } catch { return false; }
        }
        public static bool TryBeginBoardcastData(IEnumerable<Socket> targets, byte[] data, int offset, AsyncCallback callback, object state)
        {
            try { BeginBoardcastData(targets, data, offset, callback, state); return true; } catch { return false; }
        }
        public static bool TryBeginBoardcastData(IEnumerable<Socket> targets, byte[] data, AsyncCallback callback, object state)
        {
            try { BeginBoardcastData(targets, data, callback, state); return true; } catch { return false; }
        }
        public static bool TryBeginBoardcastData(IEnumerable<Socket> targets, byte[] data, int offset, int length)
        {
            try { BeginBoardcastData(targets, data, offset, length); return true; } catch { return false; }
        }
        public static bool TryBeginBoardcastData(IEnumerable<Socket> targets, byte[] data, int offset)
        {
            try { BeginBoardcastData(targets, data, offset); return true; } catch { return false; }
        }
        public static bool TryBeginBoardcastData(IEnumerable<Socket> targets, byte[] data)
        {
            try { BeginBoardcastData(targets, data); return true; } catch { return false; }
        }
    }
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

        private void AcceptAction(IAsyncResult result)
        {
            TcpClient tcpClient = server.EndAcceptTcpClient(result);
            EventedClient newClient = new EventedClient(tcpClient);

            OnClientConnected(newClient);

            if (continueAccept)
            {
                server.BeginAcceptSocket(AcceptAction, null);
            }
        }

        public EventedClient AcceptClient()
        {
            return new EventedClient(server.AcceptTcpClient());
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
            server.BeginAcceptSocket(AcceptAction, null);
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

        private void OnClientConnected(EventedClient client)
        {
            if (ClientConnected != null)
                ClientConnected.Invoke(this, new ClientConnectedArgs(client));
        }

        public event EventHandler<ClientConnectedArgs> ClientConnected;
    }
    public class EventedClient
    {
        TcpClient client;
        NetworkStream stream;

        bool continueReceive;

        public EventedClient()
        {
            client = new TcpClient();

            SendDataDelegate = SendData;
            ReceiveDataDelegate = ReceiveData;
        }
        public EventedClient(TcpClient client)
        {
            if (client == null)
                throw new ArgumentNullException("client");

            this.client = client;
            stream = client.GetStream();

            SendDataDelegate = SendData;
            ReceiveDataDelegate = ReceiveData;
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
        
        private void EventReceiveCallback(IAsyncResult result)
        {
            byte[] data = ReceiveDataDelegate.EndInvoke(result);
            if (data != null)
                OnDataReceived(data, data.Length);

            if (continueReceive)
                StartReceiveData();
        }
        private void SetStreamAction(IAsyncResult result)
        {
            stream = client.GetStream();
        }

        public IAsyncResult BeginConnect(IPAddress address, int port, AsyncCallback callback, object state)
        {
            AsyncCallback connectCallback = SetStreamAction;
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
        private Action<byte[], int, int> SendDataDelegate;
        public IAsyncResult BeginSendData(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
        {
            return SendDataDelegate.BeginInvoke(buffer, offset, size, callback, state);
        }
        public IAsyncResult BeginSendData(byte[] buffer, int offset, AsyncCallback callback, object state)
        {
            int size = buffer.Length - offset;
            return SendDataDelegate.BeginInvoke(buffer, offset, size, callback, state);
        }
        public IAsyncResult BeginSendData(byte[] buffer, AsyncCallback callback, object state)
        {
            int size = buffer.Length;
            return SendDataDelegate.BeginInvoke(buffer, 0, size, callback, state);
        }
        public IAsyncResult BeginSendData(byte[] buffer, int offset, int size)
        {
            return SendDataDelegate.BeginInvoke(buffer, offset, size, (rst) => SendDataDelegate.EndInvoke(rst), null);
        }
        public IAsyncResult BeginSendData(byte[] buffer, int offset)
        {
            int size = buffer.Length - offset;
            return SendDataDelegate.BeginInvoke(buffer, offset, size, (rst) => SendDataDelegate.EndInvoke(rst), null);
        }
        public IAsyncResult BeginSendData(byte[] buffer)
        {
            int size = buffer.Length;
            return SendDataDelegate.BeginInvoke(buffer, 0, size, (rst) => SendDataDelegate.EndInvoke(rst), null);
        }
        public void EndSendData(IAsyncResult result)
        {
            SendDataDelegate.EndInvoke(result);
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
        private Func<byte[]> ReceiveDataDelegate;
        public IAsyncResult BeginReceiveData(AsyncCallback callback, object state)
        {
            return ReceiveDataDelegate.BeginInvoke(callback, state);
        }


        public void StartReceiveData()
        {
            continueReceive = true;
            ReceiveDataDelegate.BeginInvoke(EventReceiveCallback, null);
        }
        public void StopReceiveData()
        {
            continueReceive = false;
        }

        private void OnDisconnected()
        {
            if (Disconnected != null)
                Disconnected.Invoke(this, new ClientDisconnectedArgs(this));
        }
        private void OnDataReceived(byte[] buffer, int size)
        {
            if (DataReceived != null)
                DataReceived.Invoke(this, new ClientDataReceivedArgs(this, buffer, size));
        }
        private void OnErrorReceived(byte[] buffer, int size)
        {
            if (ErrorReceived != null)
                ErrorReceived.Invoke(this, new ClientErrorReceivedArgs(this, buffer, size));
        }

        public event EventHandler<ClientDisconnectedArgs> Disconnected;
        public event EventHandler<ClientDataReceivedArgs> DataReceived;
        public event EventHandler<ClientErrorReceivedArgs> ErrorReceived;
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

    public class ClientConnectedArgs : EventArgs
    {
        public EventedClient Client;
        public ClientConnectedArgs(EventedClient client)
        {
            this.Client = client;
        }
    }
    public class ClientDisconnectedArgs : EventArgs
    {
        public EventedClient Client;
        public ClientDisconnectedArgs(EventedClient client)
        {
            this.Client = client;
        }
    }
    public class ClientDataReceivedArgs : EventArgs
    {
        public EventedClient Client;
        public byte[] Buffer;
        public int Size;
        public ClientDataReceivedArgs(EventedClient client, byte[] buffer, int size)
        {
            this.Client = client;
            this.Buffer = buffer;
            this.Size = size;
        }
    }
    public class ClientErrorReceivedArgs : EventArgs
    {
        public EventedClient Client;
        public byte[] Buffer;
        public int Size;
        public ClientErrorReceivedArgs(EventedClient client, byte[] buffer, int size)
        {
            this.Client = client;
            this.Buffer = buffer;
            this.Size = size;
        }
    }
}
