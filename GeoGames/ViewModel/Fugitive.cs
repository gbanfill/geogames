using System;
using GeoGames.Messaging;

namespace GeoGames.ViewModel
{
	public class Fugitive : BaseViewModel
    {
        public Fugitive()
        {
        }

		private string _clientId;
		public string ClientId { get { return _clientId; } set { _clientId = value; OnPropertyChanged("ClientId"); } }

		private string _username;
		public string Username { get { return _username; } set { _username = value; OnPropertyChanged("Username"); } }

		private Xamarin.Forms.Maps.Position _position;
		public Xamarin.Forms.Maps.Position Position { get { return _position; } set { _position = value; OnPropertyChanged("Position"); } }

		private double _distanceToFugitive;
		public double DistanceToFugitive { get { return _distanceToFugitive; } set { _distanceToFugitive = value; OnPropertyChanged("DistanceToFugitive"); }}

		private TimeSpan _timeToReachFugitive;
		public TimeSpan TimeToReachFugitive { get { return _timeToReachFugitive; } set { _timeToReachFugitive = value; OnPropertyChanged("TimeToReachFugitive"); }}

		public FugitiveDistanceMessage ToFugitiveDistanceMessage()
		{
			// calculate distance and reply
			FugitiveDistanceMessage msg = new FugitiveDistanceMessage()
			{
				DistanceInM = DistanceToFugitive,
				TimeToReach = TimeToReachFugitive,
				FugitiveClientId = ClientId
            };

			return msg;
		}

		public CaughtMessage ToCaughtMessage()
		{
			CaughtMessage msg = new CaughtMessage()
			{
				DistanceInM = DistanceToFugitive,
				FugitiveClientId = ClientId,

			};
			return msg;
		}
	}
}
