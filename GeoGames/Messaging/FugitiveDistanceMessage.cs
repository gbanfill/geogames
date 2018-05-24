using System;
namespace GeoGames.Messaging
{
    public class FugitiveDistanceMessage : BaseMessage
    {
        public FugitiveDistanceMessage()
        {
        }

        public double DistanceInM { get; set; }

        public TimeSpan TimeToReach { get; set; }

        public override string MessageType { get { return "FugitiveDistance"; } }
    }
}
