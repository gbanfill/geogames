using System;
namespace GeoGames.Messaging
{
    public class SurrenderMessage : BaseMessage
    {
        public SurrenderMessage()
        {
        }

        public string UserName { get; set; }
    }
}
