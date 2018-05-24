using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Realtime.Messaging;

namespace GeoGames.Messaging
{
    public class MessagingManager
    {
 
        public MessagingManager(string username)
        {
            UserName = username;
            InitializeMessaging();
        }

        public string UserName { get; set; }

        #region messaging

        protected OrtcClient client;

        string applicationKey = "Zb47xd";
        string authenticationToken = "testing";

        public string Channel { get; set; }

        private void InitializeMessaging()
        {
            client = new OrtcClient();
            client.ClusterUrl = "http://ortc-developers.realtime.co/server/2.1";
            client.OnConnected += ortc_OnConnected;
            client.OnDisconnected += ortc_OnDiconnected;
            client.OnSubscribed += ortc_OnSubscribed;
            client.OnException += ortc_OnException;
            client.ConnectionMetadata = "Xamarin-" + new Random().Next(1000);
            client.HeartbeatTime = 2;

            Connect();
        }

        public void Connect()
        {
            client.Connect(applicationKey, authenticationToken);
        }

        private void ortc_OnConnected(object sender)
        {
           
        }

        private void ortc_OnDiconnected(object sender)
        {
            
        }

        private void OnMessageCallback(object sender, string channel, string message)
        {
            var decoded = System.Web.HttpUtility.UrlDecode(message);
            var msg = JsonConvert.DeserializeObject(decoded);
           
            // convert message into appropaite opbject
            // RaiseFugutiveLocationRecieved(msg);
        }
        private void ortc_OnSubscribed(object sender, string channel)
        {
            
        }

        private void ortc_OnException(object sender, Exception ex)
        {
           
        }

        #endregion


        #region Fugitive Location

        /// <summary>
        /// Sends the fugitive location.
        /// </summary>
        /// <returns><c>true</c>, if fugitive location was sent, <c>false</c> otherwise.</returns>
        /// <param name="message">Message.</param>
        public bool SendFugitiveLocation(FugitiveLocationMessage message)
        {
            EnsureUsername(message);

            if (client.IsConnected && client.IsSubscribed(Channel))
            {
                string data = System.Web.HttpUtility.UrlEncode( JsonConvert.SerializeObject(message));
                client.Send(Channel, data);
            }
            else
            {
                //throw new Exception("not connected. Call Join Game first");
            }
            return true;
        }

        public delegate void FugitiveLocationEventHandler(object sender, MessageEventArgs<FugitiveLocationMessage> e);

        public event FugitiveLocationEventHandler FugutiveLocationRecieved;

        protected virtual void RaiseFugutiveLocationRecieved(FugitiveLocationMessage message)
        {
            if (FugutiveLocationRecieved != null)
                FugutiveLocationRecieved(this, new MessageEventArgs<FugitiveLocationMessage>(message));
        }

        #endregion

        #region join game

        /// <summary>
        /// Sends the join game message that adds the user into the game and joins the appropiate channels
        /// </summary>
        /// <returns><c>true</c>, if join game was sent, <c>false</c> otherwise.</returns>
        /// <param name="message">Message.</param>
        public bool SendJoinGame(JoinGameMessage message)
        {
            EnsureUsername(message);
            Channel = "GeoGame-Testing";
            client.Subscribe(Channel, true, OnMessageCallback);
            return true;
        }

        public delegate void JoinGameEventHandler(object sender, MessageEventArgs<JoinGameMessage> e);

        public event JoinGameEventHandler JoinGameRecieved;

        protected virtual void RaiseJoinGameRecieved(JoinGameMessage message)
        {
            if (JoinGameRecieved != null)
                JoinGameRecieved(this, new MessageEventArgs<JoinGameMessage>(message));
        }

        #endregion

        #region surrender

        /// <summary>
        /// Sends the surrender message
        /// </summary>
        /// <returns><c>true</c>, if surrender was sent, <c>false</c> otherwise.</returns>
        /// <param name="message">Message.</param>
        public bool SendSurrender(SurrenderMessage message)
        {
            EnsureUsername(message);

            return true;
        }

        public delegate void SurrenderEventHandler(object sender, MessageEventArgs<SurrenderMessage> e);

        public event SurrenderEventHandler SurrenderRecieved;

        protected virtual void RaiseSurrenderRecieved(SurrenderMessage message)
        {
            if (SurrenderRecieved != null)
                SurrenderRecieved(this, new MessageEventArgs<SurrenderMessage>(message));
        }

        #endregion


        #region Fugitive Distance

        /// <summary>
        /// Sends the surrender message
        /// </summary>
        /// <returns><c>true</c>, if surrender was sent, <c>false</c> otherwise.</returns>
        /// <param name="message">Message.</param>
        public bool SendFugitiveDistance(FugitiveDistanceMessage message)
        {
            EnsureUsername(message);

            return true;
        }

        public delegate void FugitiveDistanceEventHandler(object sender, MessageEventArgs<FugitiveDistanceMessage> e);

        public event FugitiveDistanceEventHandler FugutiveDistanceRecieved;

        protected virtual void RaiseFugutiveDistanceRecieved(FugitiveDistanceMessage message)
        {
            if (FugutiveDistanceRecieved != null)
                FugutiveDistanceRecieved(this, new MessageEventArgs<FugitiveDistanceMessage>(message));
        }

        #endregion

        private void EnsureUsername(BaseMessage message)
        {
            message.TimeStamp = DateTime.UtcNow;
            message.Username = UserName;
        }
    }
}
