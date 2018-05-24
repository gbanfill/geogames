using System;
namespace GeoGames.Messaging
{
    public class FugitiveLocationMessage : BaseMessage
    {
        public FugitiveLocationMessage()
        {
        }

        public double Latitide { get; set; }

        public double Longitude { get; set; }


        public override string MessageType { get { return "FugitiveLocation"; } }
    }
}
