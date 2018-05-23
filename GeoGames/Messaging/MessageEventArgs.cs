using System;
namespace GeoGames.Messaging
{
    public class FugitiveDistanceMessageEventArgs
    {
        public FugitiveDistanceMessageEventArgs(FugitiveDistanceMessage message)
        {
        }
        public FugitiveDistanceMessage Message { get; set; }
    }
}
