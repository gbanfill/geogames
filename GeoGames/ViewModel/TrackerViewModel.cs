using System;
using System.Collections.ObjectModel;
using GeoGames.Messaging;
using Xamarin.Forms.Maps;
using System.Linq;
using Plugin.Geolocator.Abstractions;
using GeoGames.Extensions;

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

        private bool _canStartGame;
        public bool CanStartGame
        {
            get { return _canStartGame; }
            set
            {
                _canStartGame = value;
                OnPropertyChanged("CanStartGame");
            }
        }

		private Plugin.Geolocator.Abstractions.Position _position;
        public Plugin.Geolocator.Abstractions.Position Position
        {
            get { return _position; }
            set
            {
                _position = value;
                OnPropertyChanged("Position");
            }
        }

		public void CreateMessaging(string username)
		{
			if (Messaging == null)
			{
				Messaging = new MessagingManager(username);

				Messaging.Connected += _messaging_Connected;
				Messaging.FugutiveLocationRecieved += _messaging_FugutiveLocationRecieved;
				Messaging.HelloRecieved += Messaging_HelloRecieved;
				Messaging.SurrenderRecieved += Messaging_SurrenderRecieved;
			}
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
			if (ViewModelLocator.TrackerViewModel.Position != null)
			{
				var fugitiveGeolocatorPosition = fugitive.Position.ToGeolocatorPosition();
                
				var distanceInKM = ViewModelLocator.TrackerViewModel.Position.CalculateDistance(fugitiveGeolocatorPosition, GeolocatorUtils.DistanceUnits.Kilometers);
				fugitive.DistanceToFugitive = distanceInKM * 1000;
				fugitive.TimeToReachFugitive = TimeSpan.FromSeconds(fugitive.DistanceToFugitive / FIVE_METERS_PER_SECOND);
				if (fugitive.DistanceToFugitive > CAUGHT_DISTANCE)
				{
					FugitiveDistanceMessage msg = fugitive.ToFugitiveDistanceMessage();
					ViewModelLocator.TrackerViewModel.Messaging.SendFugitiveDistance(msg);
				}else{
					CaughtMessage msg = fugitive.ToCaughtMessage();

                    ViewModelLocator.TrackerViewModel.Messaging.SendCaughtMessage(msg);
				}
			}
        }

		void Messaging_JoinGameRecieved(object sender, MessageEventArgs<JoinGameMessage> e)
		{
		}

		void Messaging_HelloRecieved(object sender, MessageEventArgs<HelloMessage> e)
		{
			var fugitive = ViewModelLocator.TrackerViewModel.FugitiveCollection.FirstOrDefault(f => f.ClientId == e.Message.ClientId);
			if (fugitive == null)
			{
				ViewModelLocator.TrackerViewModel.FugitiveCollection.Add(
					new Fugitive() { ClientId = e.Message.ClientId, Username = e.Message.Username }
				);
			}
            if (ViewModelLocator.TrackerViewModel.FugitiveCollection.Count > 0)
            {
                CanStartGame = true;
            }
		}

		void Messaging_SurrenderRecieved(object sender, MessageEventArgs<SurrenderMessage> e)
		{
			var fugitive = ViewModelLocator.TrackerViewModel.FugitiveCollection.FirstOrDefault(f => f.ClientId == e.Message.ClientId);
			if (fugitive != null)
			{
				ViewModelLocator.TrackerViewModel.FugitiveCollection.Remove(fugitive);
			}
			UpdateMapPins();
		}


		private const int FIVE_METERS_PER_SECOND = 5;
		private const int CAUGHT_DISTANCE = 5;

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
			PinCollection.Clear();
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
