using System;
namespace GeoGames.Messaging
{
    public class SurrenderMessage : BaseMessage
    {
        public SurrenderMessage()
        {
        }

        public override string MessageType { get { return "Surrender"; } }
    }
}
