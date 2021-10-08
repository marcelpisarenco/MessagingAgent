using Common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Subscriber
{
    class SubscriberSocket
    {
        private Socket _socket;
        private string _topic;

        public SubscriberSocket(string topic)
        {
            _topic = topic;
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect(string ipAdress, int Port)
        {
            _socket.BeginConnect(new IPEndPoint(IPAddress.Parse(ipAdress), Port), ConnectedCallback, null);
            Console.WriteLine("Waiting for connection");
        }

        private void ConnectedCallback(IAsyncResult asyncResult)
        {
            if(_socket.Connected)
            {
                Console.WriteLine("Subscriber connected to broker");
                Subscribe();
                StartReceive();
            }
            else
            {
                Console.WriteLine("Subscriber is not connected to broker");
            }
        }

        private void Subscribe()
        {
            var data = Encoding.UTF8.GetBytes("subscribe#" + _topic);
            Send(data);
        }

        private void Send(byte[] data)
        {
            try
            {
                _socket.Send(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void StartReceive()
        {
            ConnectionInfo connection = new ConnectionInfo();
            connection.socket = _socket;
            _socket.BeginReceive(connection.Data, 0, connection.Data.Length, SocketFlags.None, ReceiveCallback, connection);
        }

        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            ConnectionInfo connectionInfo = asyncResult.AsyncState as ConnectionInfo;

            try
            {
                SocketError response;
                int buffsize = _socket.EndReceive(asyncResult, out response);
                if(response == SocketError.Success)
                {
                    byte[] payloadBytes = new byte[buffsize];
                    Array.Copy(connectionInfo.Data, payloadBytes, payloadBytes.Length);

                    PayloadHandler.Handle(payloadBytes);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                try
                {
                    connectionInfo.socket.BeginReceive(connectionInfo.Data, 0, connectionInfo.Data.Length, SocketFlags.None, ReceiveCallback, connectionInfo);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    connectionInfo.socket.Close();
                }
            }
        }


    }
}
