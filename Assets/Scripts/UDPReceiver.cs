using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class UDPReceiver
    {
        private UdpClient listener;
        private IPEndPoint groupEP;
        private Mutex mutex;
        private Queue<byte[]> messagesLocal = new Queue<byte[]>();
        private Queue<byte[]> messagesMobile = new Queue<byte[]>();
        private bool stop = false;
        public Task task;

        public UDPReceiver()
        {
            listener = new UdpClient(9000);
            groupEP = new IPEndPoint(IPAddress.Any, 9000);
            task = ReceiveMessages();
        }

        private async Task ReceiveMessages()
        {
            while(!stop)
            {
                try
                {
                    var receiveResult = await listener.ReceiveAsync();
                    lock (this)
                    {
                        if(IPAddress.IsLoopback(receiveResult.RemoteEndPoint.Address))
                        {
                            messagesLocal.Enqueue(receiveResult.Buffer);
                        }
                        else
                        {
                            messagesMobile.Enqueue(receiveResult.Buffer);
                        }
                    }
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (SocketException ex)
                {
                    Debug.LogError($"Error code: {ex.SocketErrorCode} Message: {ex.Message}");
                }
            }
        }

        public byte[] PopLocalMessage()
        {
           lock(this)
            {
                if (messagesLocal.Count == 0)
                    return null;
                return messagesLocal.Dequeue();
            }
        }

        public byte[] PopMobileMessage()
        {
            lock (this)
            {
                if (messagesMobile.Count == 0)
                    return null;
                return messagesMobile.Dequeue();
            }
        }

        public void Close()
        {
            lock(this)
            {
                stop = true;
            }
            listener.Close();
        }
    }
}
