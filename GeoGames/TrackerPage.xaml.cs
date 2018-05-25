using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GeoGames.Messaging;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Linq;
using GeoGames.ViewModel;

namespace GeoGames
{
    public partial class TrackerPage : ContentPage
    {
        public TrackerPage()
        {
            InitializeComponent();
			BindingContext = ViewModelLocator.TrackerViewModel;
			ViewModelLocator.TrackerViewModel.CreateMessaging("testing");
        }

     
            
		protected override async void OnAppearing()
		{
			base.OnAppearing();
			ViewModelLocator.TrackerViewModel.Messaging.Connected += _messaging_Connected;
			ViewModelLocator.TrackerViewModel.Messaging.FugutiveLocationRecieved += _messaging_FugutiveLocationRecieved;
			var position = await CrossGeolocator.Current.GetPositionAsync();
 
			MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude), Distance.FromMiles(0.1)));

		}

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
			ViewModelLocator.TrackerViewModel.Messaging.FugutiveLocationRecieved -= _messaging_FugutiveLocationRecieved;
        }

        void _messaging_Connected(object sender, EventArgs e)
        {
			ViewModelLocator.TrackerViewModel.Messaging.SendJoinGame(new JoinGameMessage());
        }

        void _messaging_FugutiveLocationRecieved(object sender, MessageEventArgs<FugitiveLocationMessage> e)
        {
			// add a pin
			var pins = ViewModelLocator.TrackerViewModel.PinCollection;
			if (pins.Any(p => p.Label == e.Message.Username))
            {
                // update
				var pin = pins.First(p => p.Label == e.Message.Username);
				pins.Remove(pin);
            }

			pins.Add(new Pin()
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
			ViewModelLocator.TrackerViewModel.Messaging.SendFugitiveDistance(msg);
        }

	}
}
