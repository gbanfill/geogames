using System;
using System.Collections.Generic;
using GeoGames.Messaging;
using GeoGames.ViewModel;
using Plugin.Share;
using Plugin.Share.Abstractions;
using Xamarin.Forms;

namespace GeoGames
{
    public partial class InviteFugitivesPage : ContentPage
    {
        public InviteFugitivesPage()
        {
            InitializeComponent();
			BindingContext = ViewModelLocator.TrackerViewModel;
			ViewModelLocator.TrackerViewModel.CreateMessaging("tracker");
        }

		async void StartGame_Clicked(object sender, System.EventArgs e)
		{
			ViewModelLocator.GameStartingViewModel = new GameStartingViewModel();
			ViewModelLocator.GameStartingViewModel.StartingDateTime = DateTime.Now.AddSeconds(10);
			ViewModelLocator.GameStartingViewModel.StartCountdownTimer();
            ViewModelLocator.TrackerViewModel.IsComplete = false;
            foreach (var f in ViewModelLocator.TrackerViewModel.FugitiveCollection)
            {
                f.IsCaught = false;
            }
			ViewModelLocator.TrackerViewModel.Messaging.SendGameStartsAt(ViewModelLocator.GameStartingViewModel.ToGameStartsMessage());
			await Navigation.PushModalAsync(new ModelGameCountDown());  
			await Navigation.PushAsync(new TrackerPage());
		}

        async void Invite_Clicked(object sender, EventArgs eventArgs)
        {
            if (ViewModelLocator.TrackerViewModel.GameId.Equals(DeepLinkingConstants.DEFAULT_GAME))
            {
                ViewModelLocator.TrackerViewModel.GameId = Guid.NewGuid().ToString();
                ViewModelLocator.TrackerViewModel.Messaging.Channel = ViewModelLocator.TrackerViewModel.GameId;
                ViewModelLocator.TrackerViewModel.Messaging.SendJoinGame(new JoinGameMessage());

            }
            var url = DeepLinkingConstants.GenerateShareURL(ViewModelLocator.TrackerViewModel.GameId);

            var title = "Join my game";
            var message = "Join my game of Geogames fugitive. Install the app or open the link in the app to start!";
            var sharemessage = new ShareMessage();
            sharemessage.Url = url;
            sharemessage.Title = title;
            sharemessage.Text = message;
            await CrossShare.Current.Share(sharemessage);
        }
        protected override bool OnBackButtonPressed()
        {
            ViewModelLocator.TrackerViewModel.CloseMessaging();
            return base.OnBackButtonPressed();
        }
    }
}
