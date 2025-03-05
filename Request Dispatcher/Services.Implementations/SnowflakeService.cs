using System;
using System.Threading;
using Request_Dispatcher.Services.Interfaces;

namespace Request_Dispatcher.Services.Imlpementations
{
    public class SnowflakeService : ISnowflakeService
    {
        private static readonly DateTime epoch = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private const int WorkerIdBits = 5;
        private const int DatacenterIdBits = 5;
        private const int SequenceBits = 12;

        private const long MaxWorkerId = -1L ^ (-1L << WorkerIdBits);         // 31
        private const long MaxDatacenterId = -1L ^ (-1L << DatacenterIdBits); // 31

        private const int WorkerIdShift = SequenceBits;                        // 12
        private const int DatacenterIdShift = SequenceBits + WorkerIdBits;       // 17
        private const int TimestampLeftShift = SequenceBits + WorkerIdBits + DatacenterIdBits; // 22
        private const long SequenceMask = -1L ^ (-1L << SequenceBits);

        private long lastTimestamp = -1L;
        private long sequence = 0L;

        public long WorkerId { get; private set; }
        public long DatacenterId { get; private set; }

        private readonly object lockObj = new object();

        public SnowflakeService(long workerId, long datacenterId)
        {
            if (workerId < 0 || workerId > MaxWorkerId)
                throw new ArgumentException($"workerId must be between 0 and {MaxWorkerId}");
            if (datacenterId < 0 || datacenterId > MaxDatacenterId)
                throw new ArgumentException($"datacenterId must be between 0 and {MaxDatacenterId}");

            WorkerId = workerId;
            DatacenterId = datacenterId;
        }

        public long NextId()
        {
            lock (lockObj)
            {
                long timestamp = GetCurrentTimestamp();

                if (timestamp < lastTimestamp)
                    throw new Exception($"Czas systemowy cofnął się. Ostatni znany czas: {lastTimestamp}, bieżący: {timestamp}");

                if (timestamp == lastTimestamp)
                {
                    sequence = (sequence + 1) & SequenceMask;
                    if (sequence == 0)
                    {
                        timestamp = WaitNextMillis(lastTimestamp);
                    }
                }
                else
                {
                    sequence = 0;
                }

                lastTimestamp = timestamp;

                long id = (timestamp << TimestampLeftShift) |
                          (DatacenterId << DatacenterIdShift) |
                          (WorkerId << WorkerIdShift) |
                          sequence;
                return id;
            }
        }

        private long GetCurrentTimestamp()
        {
            return (long)(DateTime.UtcNow - epoch).TotalMilliseconds;
        }

        private long WaitNextMillis(long lastTimestamp)
        {
            long timestamp = GetCurrentTimestamp();
            while (timestamp <= lastTimestamp)
            {
                Thread.Sleep(1);
                timestamp = GetCurrentTimestamp();
            }
            return timestamp;
        }
    }
}
