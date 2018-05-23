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
    }
}
