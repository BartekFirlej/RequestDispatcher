using Request_Dispatcher.Requests;

namespace Request_Dispatcher.Messages
{
    public class FlightMessage
    {
        public int OperatorID { get; set; }
        public int TeamID { get; set; }
        public string FlightID { get; set; }
        public int PlatoonID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public DateTime BeginTime { get; set; }
        public string? Comment { get; set; }

        public FlightMessage(FlightBeginRequest request)
        {
            OperatorID = request.OperatorID;
            TeamID = request.TeamID;
            FlightID = request.FlightID?.ToString() ?? string.Empty;
            PlatoonID = request.PlatoonID;
            X = request.X;
            Y = request.Y;
            Z = request.Z;
            BeginTime = request.BeginTime;
            Comment = request.Comment;
        }
    }

}
