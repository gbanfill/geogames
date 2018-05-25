using System;
using System.Collections.ObjectModel;
using GeoGames.Messaging;
using Xamarin.Forms.Maps;

namespace GeoGames.ViewModel
{
	public class TrackerViewModel : BaseViewModel
    {
        public TrackerViewModel()
        {
			_fugitiveUpdateFrequency = 30;
			_trackerUpdateFrequency = 30;
        }

		private int _fugitiveUpdateFrequency;
		public int FugitiveUpdateFrequency
		{
			get { return _fugitiveUpdateFrequency; }
			set
			{
				_fugitiveUpdateFrequency = value;
				OnPropertyChanged("FugitiveUpdateFrequency");
			}
		}

		private int _trackerUpdateFrequency;
        public int TrackerUpdateFrequency
        {
			get { return _trackerUpdateFrequency; }
            set
            {
				_trackerUpdateFrequency = value;
				OnPropertyChanged("TrackerUpdateFrequency");
            }
        }

		public void CreateMessaging(string username)
		{
			Messaging = new MessagingManager(username);
		}
		public MessagingManager Messaging { get; set; }

		private ObservableCollection<Pin> _pinCollection = new ObservableCollection<Pin>();
		public ObservableCollection<Pin> PinCollection { get { return _pinCollection; } set { _pinCollection = value; OnPropertyChanged("PinCollection"); } }

    }
}
