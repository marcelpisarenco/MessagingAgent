using Common;
using System.Collections.Concurrent;

namespace Broker
{
    static class PayloadStorage
    {
        private static ConcurrentQueue<PayLoad> _payloadsQueue;

        static PayloadStorage()
        {
            _payloadsQueue = new ConcurrentQueue<PayLoad>();

        }

        public static void Add(PayLoad payLoad)
        {
            _payloadsQueue.Enqueue(payLoad);
        }

        public static PayLoad GetNext()
        {
            PayLoad payLoad = null;
            _payloadsQueue.TryDequeue(out payLoad);
            return payLoad;
        }

        public static bool IsEmpty()
        {
            return _payloadsQueue.IsEmpty;
        }
    }
}
