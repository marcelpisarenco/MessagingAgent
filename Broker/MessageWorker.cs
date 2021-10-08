﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Broker
{
    class MessageWorker
    {
        private const int s = 500;
        public void DoSendMessageWork()
        {
            while(true)
            {
                while(!PayloadStorage.IsEmpty())
                {
                    var payload = PayloadStorage.GetNext();

                    if(payload != null)
                    {
                        var connections = ConnectionsStorage.GetConnectionsByTopic(payload.Topic);

                        foreach(var connection in connections)
                        {
                            var payloadString = JsonConvert.SerializeObject(payload);
                            byte[] data = Encoding.UTF8.GetBytes(payloadString);

                            connection.socket.Send(data);
                        }
                    }
                }
                Thread.Sleep(s);
            }
        }
    }
}
