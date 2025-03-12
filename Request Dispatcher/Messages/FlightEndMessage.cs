namespace Request_Dispatcher.Messages
{
    public class FlightEndMessage
    {
        public long FlightID { get; set; }
        public DateTime EndTime { get; set; }
    }
}
