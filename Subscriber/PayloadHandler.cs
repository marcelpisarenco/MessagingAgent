using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subscriber
{
    class PayloadHandler
    {
        public static void Handle(byte[] payloadBytes)
        {
            var PayloadString = Encoding.UTF8.GetString(payloadBytes);
            var payload = JsonConvert.DeserializeObject<PayLoad>(PayloadString);

            Console.WriteLine(payload.Message);
        }
    }
}
