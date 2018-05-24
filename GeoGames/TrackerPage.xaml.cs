using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GeoGames.Messaging;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Linq;

namespace GeoGames
{
    public partial class TrackerPage : ContentPage
    {
        public TrackerPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private ObservableCollection<Pin> _pinCollection = new ObservableCollection<Pin>();
        public ObservableCollection<Pin> PinCollection { get { return _pinCollection; } set { _pinCollection = value; OnPropertyChanged(); } }

      
        MessagingManager _messaging = new MessagingManager("testing");


		protected override async void OnAppearing()
		{
			base.OnAppearing();
            _messaging.Connected += _messaging_Connected;
            _messaging.FugutiveLocationRecieved += _messaging_FugutiveLocationRecieved;
			var position = await CrossGeolocator.Current.GetPositionAsync();
 
			MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude), Distance.FromMiles(0.1)));

		}

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _messaging.FugutiveLocationRecieved -= _messaging_FugutiveLocationRecieved;
        }

        void _messaging_Connected(object sender, EventArgs e)
        {
            _messaging.SendJoinGame(new JoinGameMessage());
        }

        void _messaging_FugutiveLocationRecieved(object sender, MessageEventArgs<FugitiveLocationMessage> e)
        {
            // add a pin
            if (PinCollection.Any(p => p.Label == e.Message.Username))
            {
                // update
                var pin = PinCollection.First(p => p.Label == e.Message.Username);
                PinCollection.Remove(pin);
            }

                PinCollection.Add(new Pin()
                {
                    Position = new Xamarin.Forms.Maps.Position(e.Message.Latitide, e.Message.Longitude),
                    Type = PinType.Generic,
                    Label = e.Message.Username
                });


            // calculate distance and reply
            FugitiveDistanceMessage msg = new FugitiveDistanceMessage()
            {
                DistanceInM = 100,
                TimeToReach = TimeSpan.FromSeconds(30)
            };
            _messaging.SendFugitiveDistance(msg);
        }

	}
}
