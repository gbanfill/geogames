using System;
namespace GeoGames.Messaging
{
	public class GameStartsAtMessage : BaseMessage
    {
        public GameStartsAtMessage()
        {
        }

		public override string MessageType { get { return "GameStartsAt"; } }

		public DateTime GameStartsAtTime { get; set; }
    }
}
