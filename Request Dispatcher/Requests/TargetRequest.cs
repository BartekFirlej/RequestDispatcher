namespace Request_Dispatcher.Requests
{
    public class TargetRequest
    {
        public long? ObjectId { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public int OperatorId { get; set; }
        public string ObjectCategory { get; set; } = null!;
        public int TeamId { get; set; }
        public int PlatoonId { get; set; }
        public DateTime Timestamp { get; set; }
        public byte[]? Image { get; set; }
        public string? Comment { get; set; }
    }
}
