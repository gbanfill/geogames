using System;
using System.Collections.Generic;
using GeoGames.ViewModel;
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
    }
}
