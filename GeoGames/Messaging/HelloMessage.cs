using System;
namespace GeoGames.Messaging
{
	public class HelloMessage : BaseMessage
    {
        public HelloMessage()
        {
        }

		public override string MessageType { get { return "HelloMessage"; }}
	}
}
