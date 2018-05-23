using System;
using System.Collections.Generic;
using GeoGames.Messaging;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace GeoGames
{
    public partial class TrackerPage : ContentPage
    {
        public TrackerPage()
        {
            InitializeComponent();
        }

        MessagingManager _messaging = new MessagingManager("testing");


		protected override async void OnAppearing()
		{
			base.OnAppearing();

            _messaging.FugutiveLocationRecieved += _messaging_FugutiveLocationRecieved;
			var position = await CrossGeolocator.Current.GetPositionAsync();
 
			MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude), Distance.FromMiles(0.1)));

		}

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _messaging.FugutiveLocationRecieved -= _messaging_FugutiveLocationRecieved;
        }

        void _messaging_FugutiveLocationRecieved(object sender, MessageEventArgs<FugitiveLocationMessage> e)
        {
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
