using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeoGames.Messaging;
using GeoGames.ViewModel;
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
			BindingContext = ViewModelLocator.FugitiveViewModel;
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
			ViewModelLocator.FugitiveViewModel.JoinEnabled = true;         
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
            
			ViewModelLocator.FugitiveViewModel.Position = e.Position;

            var message = new FugitiveLocationMessage()
            {
                Latitide = e.Position.Latitude,
                Longitude = e.Position.Longitude
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
			ViewModelLocator.FugitiveViewModel.Distance = e.Message.DistanceInM;
			ViewModelLocator.FugitiveViewModel.Time = e.Message.TimeToReach.TotalSeconds;
            ViewModelLocator.FugitiveViewModel.SurrenderEnabled = true;
           
        }

        async void Join_Clicked(object sender, EventArgs eventArgs)
        {
			ViewModelLocator.FugitiveViewModel.JoinEnabled = false;
			ViewModelLocator.FugitiveViewModel.SurrenderEnabled = true;
           
            _messaging.SendJoinGame(new JoinGameMessage());

            await StartListeningToLocation();
        }

        async void Surrender_Clicked(object sender, EventArgs eventArgs)
        {
			ViewModelLocator.FugitiveViewModel.JoinEnabled = true;
			ViewModelLocator.FugitiveViewModel.SurrenderEnabled = false;
            _messaging.SendSurrender(new SurrenderMessage());

            await StopListeningForLocation();
        }
	}
}
