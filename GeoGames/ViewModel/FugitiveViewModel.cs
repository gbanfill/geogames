using System;
using Plugin.Geolocator.Abstractions;

namespace GeoGames.ViewModel
{
	public class FugitiveViewModel : BaseViewModel
    {
        public FugitiveViewModel()
        {
            GameId = DeepLinkingConstants.DEFAULT_GAME;
        }

		private bool _joinEnabled;
		public bool JoinEnabled
		{
			get { return _joinEnabled; }
			set { _joinEnabled = value; base.OnPropertyChanged("JoinEnabled"); }
		}

		private bool _surrenderEnabled;
		public bool SurrenderEnabled
        {
			get { return _surrenderEnabled; }
			set { _surrenderEnabled = value; base.OnPropertyChanged("SurrenderEnabled"); }
        }


		private double _distance;
        public double Distance
		{
			get { return _distance; }
			set { _distance = value; OnPropertyChanged("Distance"); }
		}

		private double _time;
        public double Time
        {
			get { return _time; }
			set { _time = value; OnPropertyChanged("Time"); }
        }

		private string _fugitiveLocationString;

		public string FugitiveLocationString {
			get { return _fugitiveLocationString; }
			set { _fugitiveLocationString = value; OnPropertyChanged("FugitiveLocationString"); }
		}
        
		private Position _position;
		public Position Position 
		{
			get { return _position; }
			set { _position = value; 
				OnPropertyChanged("Position"); 
				FugitiveLocationString = string.Format("Lat: {0} Long: {1} Accuracy: {2:0.00}" , value.Latitude,  value.Longitude, value.Accuracy);
			}
		}

		private string _fugitiveName;

        public string FugitiveName
        {
			get { return _fugitiveName; }
			set { _fugitiveName = value; OnPropertyChanged("FugitiveName"); }
        }

        public DateTime StartTime { get; set; }

        private TimeSpan _aliveFor;
        public TimeSpan AliveFor { 
            get { return _aliveFor; }
            set { _aliveFor = value; AliveForSeconds = value.TotalSeconds; OnPropertyChanged("AliveFor"); OnPropertyChanged("AliveForSeconds"); } 
        }

        public double AliveForSeconds { get; set; }

        private string _gameId;
        public string GameId {
            get { return _gameId; }
            set { _gameId = value; OnPropertyChanged("GameId"); }
        }
    }
}
