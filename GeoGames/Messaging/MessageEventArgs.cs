using System;
namespace GeoGames.Messaging
{
    public class MessageEventArgs<T>
    {
        public MessageEventArgs(T message)
        {
        }

        public T Message { get; set; }
    }
}
