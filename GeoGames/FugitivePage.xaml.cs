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

        MessagingManager _messaging = new MessagingManager("conecting");

		protected override void OnAppearing()
		{
			base.OnAppearing();
            _messaging.FugutiveDistanceRecieved += _messaging_FugutiveDistanceRecieved;
			_messaging.GameStartsAtRecieved += _messaging_GameStartsAtRecieved;
            _messaging.Connected += _messaging_Connected;		
			_messaging.CaughtRecieved += _messaging_CaughtRecieved;

		}
		protected override async void OnDisappearing()
		{
			base.OnDisappearing();
            _messaging.FugutiveDistanceRecieved -= _messaging_FugutiveDistanceRecieved;
			_messaging.GameStartsAtRecieved -= _messaging_GameStartsAtRecieved;
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

		async void _messaging_GameStartsAtRecieved(object sender, MessageEventArgs<GameStartsAtMessage> e)
		{
            ViewModelLocator.FugitiveViewModel.WaitingForGameToStart = false;
			ViewModelLocator.GameStartingViewModel = new GameStartingViewModel();
			ViewModelLocator.GameStartingViewModel.StartingDateTime = e.Message.GameStartsAtTime;
            ViewModelLocator.GameStartingViewModel.StartCountdownTimer();

            ViewModelLocator.GameStartingViewModel.OnStartedAction = async () =>
            {
                ViewModelLocator.FugitiveViewModel.GameInProgress = true;
                if (ViewModelLocator.FugitiveViewModel != null)
                {
                    ViewModelLocator.FugitiveViewModel.StartTime = DateTime.Now;
                }
                await StartListeningToLocation();

            };
            await Navigation.PushModalAsync(new ModelGameCountDown());

			
		}

		async void _messaging_CaughtRecieved(object sender, MessageEventArgs<CaughtMessage> e)
        {
			await StopListeningForLocation();
            // guard against multiple caughts 
            if (!isCaught)
            {
                isCaught = true;
                ViewModelLocator.FugitiveViewModel.AliveFor = DateTime.Now - ViewModelLocator.FugitiveViewModel.StartTime;
                await Navigation.PushModalAsync(new CaughtPage());
                SendBackButtonPressed();
            }
        }

        bool isCaught;

        void Join_Clicked(object sender, EventArgs eventArgs)
        {
			ViewModelLocator.FugitiveViewModel.JoinEnabled = false;
			ViewModelLocator.FugitiveViewModel.SurrenderEnabled = false;
            ViewModelLocator.FugitiveViewModel.WaitingForGameToStart = true;

			_messaging.UserName = ViewModelLocator.FugitiveViewModel.FugitiveName;
            _messaging.Channel = ViewModelLocator.FugitiveViewModel.GameId;
            _messaging.SendJoinGame(new JoinGameMessage());
        }

        async void Surrender_Clicked(object sender, EventArgs eventArgs)
        {
			ViewModelLocator.FugitiveViewModel.JoinEnabled = true;
			ViewModelLocator.FugitiveViewModel.SurrenderEnabled = false;
            ViewModelLocator.FugitiveViewModel.GameInProgress = false;
            _messaging.SendSurrender(new SurrenderMessage());

            await StopListeningForLocation();
        }

		async void SendNow_Clicked(object sender, EventArgs args)
		{
			ViewModelLocator.FugitiveViewModel.Position = await CrossGeolocator.Current.GetLastKnownLocationAsync();

            var message = new FugitiveLocationMessage()
            {
				Latitide = ViewModelLocator.FugitiveViewModel.Position.Latitude,
				Longitude = ViewModelLocator.FugitiveViewModel.Position.Longitude
            };
            _messaging.SendFugitiveLocation(message);
		}



	}
}
