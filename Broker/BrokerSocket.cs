using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Common;

namespace Broker
{
    class BrokerSocket
    {
        private const int Connections_Limit = 5;
        private Socket _socket; 

        public BrokerSocket()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Start(string ip, int port)
        {
            _socket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
            _socket.Listen(Connections_Limit);
            Accept();
        }

        private void Accept()
        {
            _socket.BeginAccept(AcceptedCallBack, null);
        }

        private void AcceptedCallBack(IAsyncResult asyncResult)
        {
            ConnectionInfo connection = new ConnectionInfo();

            try
            {
                connection.socket = _socket.EndAccept(asyncResult);
                connection.Adress = connection.socket.RemoteEndPoint.ToString();
                connection.socket.BeginReceive(connection.Data, 0, connection.Data.Length, SocketFlags.None, ReceiveCallback, connection);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Accept();
            }
        }

        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            ConnectionInfo connection = asyncResult.AsyncState as ConnectionInfo;

            try
            {
                Socket senderSocket = connection.socket;
                SocketError response;
                int buffSize = senderSocket.EndReceive(asyncResult, out response);
                if(response == SocketError.Success)
                {
                    byte[] payload = new byte[buffSize];
                    Array.Copy(connection.Data, payload, payload.Length);

                    PayloadHandler.Handle(payload, connection);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                try
                {
                    connection.socket.BeginReceive(connection.Data, 0, connection.Data.Length, SocketFlags.None, ReceiveCallback, connection);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    var address = connection.socket.RemoteEndPoint.ToString();
                    ConnectionsStorage.Remove(address);
                    connection.socket.Close();
                }
            }
        }


    }
}
