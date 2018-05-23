using System;
namespace GeoGames.Messaging
{
    public class FugitiveLocationMessage : BaseMessage
    {
        public FugitiveLocationMessage()
        {
        }

        public string Username { get; set; }

        public double Latitide { get; set; }

        public double Longitude { get; set; }

       
    }
}
