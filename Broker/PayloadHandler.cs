using Common;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Broker
{
    class PayloadHandler
    {
        public static void Handle(byte[] payloadBytes, ConnectionInfo connectionInfo)
        {
            var payloadString = Encoding.UTF8.GetString(payloadBytes);

            if (payloadString.StartsWith("subscribe#"))
            {
                connectionInfo.Topic = payloadString.Split("subscribe#").LastOrDefault();
                ConnectionsStorage.Add(connectionInfo);
            }
            else
            {
                PayLoad payLoad = JsonConvert.DeserializeObject<PayLoad>(payloadString);
                PayloadStorage.Add(payLoad);
            }

            Console.WriteLine(payloadString);
        }
    }
}
