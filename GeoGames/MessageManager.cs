using System;
using Plugin.Geolocator.Abstractions;
using Realtime.Messaging;

namespace GeoGames
{
    public class MessageManager
    {

        public string AuthToken = "";
        public string AppKey = "xpHiHL";
        public string ClusterUrl = "http://ortc-developers.realtime.co/server/2.1/";

		private OrtcClient ortcClient;
        public MessageManager()
        {
			ortcClient = new OrtcClient();
            ortcClient.ClusterUrl = "http://ortc-developers.realtime.co/server/2.1/";

            // Ortc client handlers
			ortcClient.OnConnected += (object sender) => {
				
			};
			ortcClient.OnSubscribed += (object sender, string channel) => {
				
			};
			ortcClient.OnException += (object sender, Exception ex) => {
				
			};

			ortcClient.Connect(AppKey, Guid.NewGuid().ToString());
        }

		private bool InGame { get; set; }

       

        private void OnMessageCallback(object sender, string channel, string message)
        {
            
        }
       
		public string GameId { get; set; }
        public void JoinGame(string gameId, string userName)
		{
			if (ortcClient.IsConnected)
			{
				GameId = "GeoGames-Fugitive-" + gameId;
				ortcClient.Subscribe(GameId, true, OnMessageCallback);
			}

		}

		public void LeaveGame(string gameId, string userName)
		{
			ortcClient.Unsubscribe(GameId);
			GameId = null;
		}

		public void SendTrackerLocation(string userName, Position location)
		{
			if (!string.IsNullOrEmpty(GameId))
			{
				throw new Exception("Not subscribed to a game. Call Join game");
			}
			var message = GetMessage(userName, location);
			ortcClient.Send(GameId, message);
		}

		private string GetMessage(string userName, Position location)
		{
			return string.Format("{0} - {1} - {2}", userName, location.Latitude, location.Longitude);
		}
        public void Surrender()
		{
			
		}

    }
}
