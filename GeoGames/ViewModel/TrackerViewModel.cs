using System;
using System.Collections.ObjectModel;
using GeoGames.Messaging;
using Xamarin.Forms.Maps;
using System.Linq;

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
            
			Messaging.Connected += _messaging_Connected;
			Messaging.FugutiveLocationRecieved += _messaging_FugutiveLocationRecieved;
			//TODO: remember to dispose of these
		}

		void _messaging_Connected(object sender, EventArgs e)
        {
            ViewModelLocator.TrackerViewModel.Messaging.SendJoinGame(new JoinGameMessage());
        }

		void _messaging_FugutiveLocationRecieved(object sender, MessageEventArgs<FugitiveLocationMessage> e)
        {
            // add fugitive
            var fugitive = ViewModelLocator.TrackerViewModel.FugitiveCollection.FirstOrDefault(p => p.ClientId == e.Message.ClientId);
            if (fugitive != null)
            {
                // update
                fugitive.Position = new Xamarin.Forms.Maps.Position(e.Message.Latitide, e.Message.Longitude);
            }
            else
            {
                // add
                fugitive = new Fugitive()
                {
                    ClientId = e.Message.ClientId,
                    Username = e.Message.Username,
                    Position = new Xamarin.Forms.Maps.Position(e.Message.Latitide, e.Message.Longitude),
                };
                ViewModelLocator.TrackerViewModel.FugitiveCollection.Add(fugitive);
            }
            ViewModelLocator.TrackerViewModel.UpdateMapPins();

            fugitive.DistanceToFugitive = 100;
            fugitive.TimeToReachFugitive = TimeSpan.FromSeconds(30);
            FugitiveDistanceMessage msg = fugitive.ToFugitiveDistanceMessage();

            ViewModelLocator.TrackerViewModel.Messaging.SendFugitiveDistance(msg);
        }

		public MessagingManager Messaging { get; set; }

		private ObservableCollection<Pin> _pinCollection = new ObservableCollection<Pin>();
        
		public ObservableCollection<Pin> PinCollection { 
			get { 
				return _pinCollection; 
                } 
			set { 
				_pinCollection = value; 
				OnPropertyChanged("PinCollection"); } 
		}
        
		private ObservableCollection<Fugitive> _fugitiveCollection = new ObservableCollection<Fugitive>();
		public ObservableCollection<Fugitive> FugitiveCollection { 
			get { return _fugitiveCollection; } 
			set { 
				_fugitiveCollection = value;

				OnPropertyChanged("FugitiveCollection");
			} }

		internal void UpdateMapPins()
		{
			foreach(var f in FugitiveCollection)
			{
				PinCollection.Add(new Pin()
				{
					Position = f.Position,
					Label = f.Username,
					Type = PinType.Generic

				});
			}
		}
	}
}
