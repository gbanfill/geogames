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
			await Navigation.PopModalAsync();
		}
    }
}
