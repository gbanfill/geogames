using System;
using Plugin.Geolocator.Abstractions;

namespace GeoGames.ViewModel
{
	public class FugitiveViewModel : BaseViewModel
    {
        public FugitiveViewModel()
        {
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
				FugitiveLocationString = string.Format("Lat: {0} Long: {1} Accuracy: {2}" , value.Latitude,  value.Longitude, value.Accuracy);
			}
		}


    }
}
