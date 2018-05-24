using System;
using Newtonsoft.Json;

namespace GeoGames.Messaging
{
    public abstract class BaseMessage
    {
        public BaseMessage()
        {
        }

       

        public DateTime TimeStamp { get; set; }

        public string Username { get; set; }

        public abstract string MessageType { get; }

        public string ClientId { get; set; }
    }
}
