using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            RaiseConnected();
        }

        public delegate void ConnectedEventHandler(object sender, EventArgs e);

        public event ConnectedEventHandler Connected;

        protected virtual void RaiseConnected()
        {
            if (Connected != null)
                Connected(this, new EventArgs());
        }

        private void ortc_OnDiconnected(object sender)
        {
            
        }

        public class MessageConvertor : JsonConverter
        {
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                JToken jObject = JToken.ReadFrom(reader);
                var type = jObject["MessageType"].ToString();

                BaseMessage message;
                switch (type)
                {
                    case "FugitiveDistance":
                        message = new FugitiveDistanceMessage();
                        break;
                    case "FugitiveLocation":
                        message = new FugitiveLocationMessage();
                        break;
					case "GameStartsAt":
                        message = new GameStartsAtMessage();
                        break;
					case "HelloMessage":
						message = new HelloMessage();
                        break;
					case "Surrender":
						message = new SurrenderMessage();
                        break;
					case "CaughtMessage":
						message = new CaughtMessage();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                serializer.Populate(jObject.CreateReader(), message);

                return message;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                JToken t = JToken.FromObject(value);

                t.WriteTo(writer);
            }

            public override bool CanConvert(Type objectType)
            {
                return objectType.Equals(typeof(BaseMessage)) || objectType.IsSubclassOf(typeof(BaseMessage));
            }
        }

        private void OnMessageCallback(object sender, string channel, string message)
        {
            var decoded = System.Web.HttpUtility.UrlDecode(message);
            var msg = JsonConvert.DeserializeObject<BaseMessage>(decoded, new MessageConvertor());

            if (msg.ClientId != client.SessionId)
            {
                // convert message into appropaite opbject
                switch (msg.MessageType)
                {
                    case "FugitiveLocation":
                        RaiseFugutiveLocationRecieved((FugitiveLocationMessage)msg);
                        break;
                    case "FugitiveDistance":
                        RaiseFugutiveDistanceRecieved((FugitiveDistanceMessage)msg);
                        break;
                    case "JoinGame":
                        RaiseJoinGameRecieved((JoinGameMessage)msg);
                        break;
                    case "Surrender":
                        RaiseSurrenderRecieved((SurrenderMessage)msg);
                        break;
					case "GameStartsAt":
                        RaiseGameStartsAtRecieved((GameStartsAtMessage)msg);
                        break;
					case "HelloMessage":
						RaiseHelloRecieved((HelloMessage)msg);
                        break;
					case "CaughtMessage":
						RaiseCaughtRecieved((CaughtMessage)msg);
						break;
                }
            }
        }
        private void ortc_OnSubscribed(object sender, string channel)
        {
			SendHello(new HelloMessage());
        }

        private void ortc_OnException(object sender, Exception ex)
        {
           
        }

        #endregion

		#region Hello

        /// <summary>
        /// Sends the fugitive location.
        /// </summary>
        /// <returns><c>true</c>, if fugitive location was sent, <c>false</c> otherwise.</returns>
        /// <param name="message">Message.</param>
        public bool SendHello(HelloMessage message)
        {
            EnsureUsername(message);

            if (client.IsConnected && client.IsSubscribed(Channel))
            {
                string data = System.Web.HttpUtility.UrlEncode(JsonConvert.SerializeObject(message));
                client.Send(Channel, data);
            }
            else
            {
                throw new Exception("not connected. Call Join Game first");
            }
            return true;
        }

        public delegate void HelloEventHandler(object sender, MessageEventArgs<HelloMessage> e);

		public event HelloEventHandler HelloRecieved;

		protected virtual void RaiseHelloRecieved(HelloMessage message)
        {
			if (HelloRecieved != null)
				HelloRecieved(this, new MessageEventArgs<HelloMessage>(message));
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
                throw new Exception("not connected. Call Join Game first");
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
			if (client.IsSubscribed(Channel))
			{
				SendHello(new HelloMessage());
			}
			else
			{
				client.Subscribe(Channel, true, OnMessageCallback);
			}
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

			if (client.IsConnected && client.IsSubscribed(Channel))
            {
                string data = System.Web.HttpUtility.UrlEncode(JsonConvert.SerializeObject(message));
                client.Send(Channel, data);
            }
            else
            {
                throw new Exception("not connected. Call Join Game first");
            }

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

			if (client.IsConnected && client.IsSubscribed(Channel))
            {
                string data = System.Web.HttpUtility.UrlEncode(JsonConvert.SerializeObject(message));
                client.Send(Channel, data);
            }
            else
            {
                throw new Exception("not connected. Call Join Game first");
            }

            return true;
        }

        public delegate void FugitiveDistanceEventHandler(object sender, MessageEventArgs<FugitiveDistanceMessage> e);

        public event FugitiveDistanceEventHandler FugutiveDistanceRecieved;
        
        protected virtual void RaiseFugutiveDistanceRecieved(FugitiveDistanceMessage message)
        {
			if (message.FugitiveClientId == client.SessionId)
			{
				// only dispatch this message if it is intended for this session
				if (FugutiveDistanceRecieved != null)
					FugutiveDistanceRecieved(this, new MessageEventArgs<FugitiveDistanceMessage>(message));
			}
        }

        #endregion

		#region SendCaughtMessage
		/// <summary>
        /// Sends the surrender message
        /// </summary>
        /// <returns><c>true</c>, if surrender was sent, <c>false</c> otherwise.</returns>
        /// <param name="message">Message.</param>
		public bool SendCaughtMessage(CaughtMessage message)
        {
            EnsureUsername(message);

            if (client.IsConnected && client.IsSubscribed(Channel))
            {
                string data = System.Web.HttpUtility.UrlEncode(JsonConvert.SerializeObject(message));
                client.Send(Channel, data);
            }
            else
            {
                throw new Exception("not connected. Call Join Game first");
            }

            return true;
        }

        public delegate void CaughtEventHandler(object sender, MessageEventArgs<CaughtMessage> e);

		public event CaughtEventHandler CaughtRecieved;

		protected virtual void RaiseCaughtRecieved(CaughtMessage message)
        {
            if (message.FugitiveClientId == client.SessionId)
            {
                // only dispatch this message if it is intended for this session
				if (CaughtRecieved != null)
					CaughtRecieved(this, new MessageEventArgs<CaughtMessage>(message));
            }
        }
		#endregion

		#region SendGameStartsAt
		public bool SendGameStartsAt(GameStartsAtMessage message)
		{
			EnsureUsername(message);

            if (client.IsConnected && client.IsSubscribed(Channel))
            {
                string data = System.Web.HttpUtility.UrlEncode(JsonConvert.SerializeObject(message));
                client.Send(Channel, data);
            }
            else
            {
                throw new Exception("not connected. Call Join Game first");
            }
			return true;

		}

		public delegate void GameStartsAtEventHandler(object sender, MessageEventArgs<GameStartsAtMessage> e);

		public event GameStartsAtEventHandler GameStartsAtRecieved;

		protected virtual void RaiseGameStartsAtRecieved(GameStartsAtMessage message)
        {
			if (GameStartsAtRecieved != null)
				GameStartsAtRecieved(this, new MessageEventArgs<GameStartsAtMessage>(message));
        }
		#endregion

        private void EnsureUsername(BaseMessage message)
        {
            message.TimeStamp = DateTime.UtcNow;
            message.Username = UserName;
            message.ClientId = client.SessionId;
        }
    }
}
