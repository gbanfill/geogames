using System;
namespace GeoGames.Messaging
{
    public class BaseMessage
    {
        public BaseMessage()
        {
        }

        public DateTime TimeStamp { get; set; }

        public string Username { get; set; }
    }
}
