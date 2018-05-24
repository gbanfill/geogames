using System;
namespace GeoGames.Messaging
{
    public class MessageEventArgs<T>
    {
        public MessageEventArgs(T message)
        {
            Message = message;
        }

        public T Message { get; set; }
    }
}
