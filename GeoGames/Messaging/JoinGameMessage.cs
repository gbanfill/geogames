using System;
namespace GeoGames.Messaging
{
    public class JoinGameMessage : BaseMessage
    {
        public JoinGameMessage()
        {
        }


        public override string MessageType { get { return "JoinGame"; } }
    }
}
