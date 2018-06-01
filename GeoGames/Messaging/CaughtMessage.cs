using System;
namespace GeoGames.Messaging
{
	public class CaughtMessage : BaseMessage
    {
        public CaughtMessage()
        {
        }

		public double DistanceInM { get; set; }

		public string FugitiveClientId { get; set; }

		public override string MessageType { get { return "CaughtMessage"; } }
    }
}
