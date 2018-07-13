using System;
using System.Collections.Generic;
using GeoGames.ViewModel;
using Xamarin.Forms;

namespace GeoGames
{
    public partial class CaughtPage : ContentPage
    {
        public CaughtPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.FugitiveViewModel;
        }

		public async void Dismiss_Clicked(object sender, EventArgs eventArgs)
		{
             // reset back so we can play again
            ViewModelLocator.FugitiveViewModel.GameInProgress = false;
            ViewModelLocator.FugitiveViewModel.WaitingForGameToStart = true;
            ViewModelLocator.FugitiveViewModel.Distance = 0;
            ViewModelLocator.FugitiveViewModel.Time = 0;
            ViewModelLocator.FugitiveViewModel.IsCaught = false;
			await Navigation.PopModalAsync();
		}
    }
}
