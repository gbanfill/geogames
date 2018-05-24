using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeoGames.Messaging;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;

namespace GeoGames
{
    public partial class FugitivePage : ContentPage
    {
        public FugitivePage()
        {
            InitializeComponent();
        }

        MessagingManager _messaging = new MessagingManager("testing");

		protected override void OnAppearing()
		{
			base.OnAppearing();
            _messaging.FugutiveDistanceRecieved += _messaging_FugutiveDistanceRecieved;
            _messaging.Connected += _messaging_Connected;
		

		}
		protected override async void OnDisappearing()
		{
			base.OnDisappearing();
            _messaging.FugutiveDistanceRecieved -= _messaging_FugutiveDistanceRecieved;
            _messaging.Connected -= _messaging_Connected;
			await StopListeningForLocation();
		}

        void _messaging_Connected(object sender, EventArgs eventArgs)
        {
            join.IsEnabled = true;
        }


        async Task StartListeningToLocation()
        {
            if (CrossGeolocator.Current.IsListening)
                return;

            await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(1), 1);

            CrossGeolocator.Current.PositionChanged += PositionChanged;
            CrossGeolocator.Current.PositionError += PositionError;
        }
      
		private void PositionChanged(object sender, PositionEventArgs e)
        {

            //If updating the UI, ensure you invoke on main thread
            var position = e.Position;
            var output = "Lat: " + position.Latitude + " Long: " + position.Longitude + " Accuracy: " + position.Accuracy;
			debug.Text = output;

            var message = new FugitiveLocationMessage()
            {
                Latitide = position.Latitude,
                Longitude = position.Longitude
            };
            _messaging.SendFugitiveLocation(message);
        }

        private void PositionError(object sender, PositionErrorEventArgs e)
        {
           
            //Handle event here for errors
        }

        async Task StopListeningForLocation()
        {
            if (!CrossGeolocator.Current.IsListening)
                return;

            await CrossGeolocator.Current.StopListeningAsync();

            CrossGeolocator.Current.PositionChanged -= PositionChanged;
            CrossGeolocator.Current.PositionError -= PositionError;
        }

        void _messaging_FugutiveDistanceRecieved(object sender, MessageEventArgs<FugitiveDistanceMessage> e)
        {
            distance.Text = string.Format("{0}m", e.Message.DistanceInM);
            time.Text = e.Message.TimeToReach.TotalSeconds.ToString();
        }

        async void Join_Clicked(object sender, EventArgs eventArgs)
        {
            join.IsEnabled = false;
            surrender.IsEnabled = true;
            _messaging.SendJoinGame(new JoinGameMessage());

            await StartListeningToLocation();
        }

        async void Surrender_Clicked(object sender, EventArgs eventArgs)
        {
            surrender.IsEnabled = false;
            join.IsEnabled = true;
            _messaging.SendSurrender(new SurrenderMessage());

            await StopListeningForLocation();
        }
	}
}
